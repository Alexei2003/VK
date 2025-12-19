using System.Collections.Concurrent;
using System.Data;
using System.Numerics;

using ClosedXML.Excel;

using DataSet;

using NeuralNetwork;

using Other.Tags.Collections;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace AddDataInDataSet
{
    public static class WorkWithDirectory
    {
#if DEBUG
        public const string MainDirectory = "E:\\WPS\\VK\\AddDataInDataSet\\bin\\Debug\\net9.0-windows10.0.22621.0\\DATA_SET\\";
#else
        public const string MainDirectory = "E:\\WPS\\NeuralNetwork-ImageClassifier\\DataSet\\ARTS\\";
#endif
        public const string NewPath = MainDirectory + $"New";
        public const string OriginalPath = MainDirectory + $"Original";
        public const string SmallPath = MainDirectory + $"Small";

        private static void DirectoryMove(string source, string destination, bool checkSimilar = false, bool deleteOriginal = false, bool changeResolution = false)
        {
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            var sourceInfo = new DirectoryInfo(source);
            foreach (var src in sourceInfo.GetFiles())
            {
                var similar = checkSimilar && Similar(src, destination);

                if (!similar && !File.Exists(Path.Combine(destination, src.Name)))
                {
                    if (changeResolution)
                    {
                        using var imageOriginal = Image.Load<Rgb24>(src.FullName);
                        using var image = DataSetImage.ChangeResolution224x224(imageOriginal);
                        image.SaveAsJpeg(Path.Combine(destination, src.Name));
                    }
                    else
                    {
                        src.MoveTo(Path.Combine(destination, src.Name));
                    }
                }
                else
                {
                    src.Delete();
                }

            }
            if (deleteOriginal)
            {
                Directory.Delete(source, true);
            }
        }

        private static readonly ConcurrentDictionary<string, Image<Rgb24>> _cacheImages = [];
        private static bool Similar(FileInfo src, string destination)
        {
            using var srcImage = Image.Load<Rgb24>(src.FullName);
            var srcImageCache = DataSetImage.ChangeResolution(srcImage, 100);
            _cacheImages.TryAdd(src.Name, srcImageCache);

            var destinationInfo = new DirectoryInfo(destination);
            foreach (var dest in destinationInfo.GetFiles())
            {
                var destImage = _cacheImages.GetValueOrDefault(dest.Name);
                if (destImage == null)
                {
                    using var destImageTmp = Image.Load<Rgb24>(dest.FullName);
                    var destImageCache = DataSetImage.ChangeResolution(destImageTmp, 100);
                    _cacheImages.TryAdd(dest.Name, destImageCache);
                    destImage = destImageCache;
                }


                if (DataSetImage.IsSimilarImage(srcImage, destImage))
                {
                    return true;
                }
            }

            return false;
        }

        private static string GetDirectoryName(string dir)
        {
            return dir.Split('\\', StringSplitOptions.RemoveEmptyEntries)[^1];
        }

        public static void MoveDataFromNewToOriginal(int[] count)
        {
            var tagDirectories = Directory.GetDirectories(NewPath);

            Parallel.ForEach(tagDirectories, tag =>
            {
                DirectoryMove(tag, Path.Combine(OriginalPath, GetDirectoryName(tag)), checkSimilar: true, deleteOriginal: true, changeResolution: false);

                count[0]++;
            });
        }

        public static void FixDataInOriginal(int[] count)
        {
            var tagDirectories = Directory.GetDirectories(OriginalPath);

            var tagList = new TagHashSet();

            Parallel.ForEach(tagDirectories, tag =>
            {
                var name = GetDirectoryName(tag);

                if (tagList.ContainTag(name))
                {
                    var sourceInfo = new DirectoryInfo(tag);
                    var filesArr = sourceInfo.GetFiles();
                    if (filesArr.Length < 100)
                    {
                        DirectoryMove(tag, Path.Combine(SmallPath, name), deleteOriginal: true);
                    }
                    else
                    {
                        var arrSkip = new[] { "#nsfw", "#original", "#bad_drawing" };
                        var countSkip = 1000;
                        if (arrSkip.Contains(name))
                        {
                            countSkip *= tagDirectories.Length;
                        }
                        for (var i = 0; i < filesArr.Length - countSkip; i++)
                        {
                            filesArr[i].Delete();
                        }
                    }
                }
                else
                {
                    Console.WriteLine(tag);
                }

                count[0]++;
            });
        }

        public static void GetAccuracyKTopClassesOriginal(int[] count)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet1");
            var dataTable = new DataTable();

            var countP5 = int.Max((int)(NeuralNetworkWorker.Labels.Length * 0.05), 1);
            var countP10 = int.Max((int)(NeuralNetworkWorker.Labels.Length * 0.10), 1);

            dataTable.Columns.Add("Тег", typeof(string));
            dataTable.Columns.Add("Количество объектов", typeof(int));
            dataTable.Columns.Add("Точность k=1", typeof(float));
            dataTable.Columns.Add($"Точность 5% k={countP5}", typeof(float));
            dataTable.Columns.Add($"Точность 10% k={countP10}", typeof(float));

            int counter = 0;
            var threadIndex = new ThreadLocal<int>(() => Interlocked.Increment(ref counter) - 1);

            Parallel.For(0, NeuralNetworkWorker.Labels.Length, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, i =>
            {
                var tagOriginal = NeuralNetworkWorker.Labels[i];

                var tagDirectory = Path.Combine(OriginalPath, tagOriginal);

                if (Directory.Exists(tagDirectory))
                {
                    var countTrueK1 = 0;
                    var countTrueP5 = 0;
                    var countTrueP10 = 0;
                    var countAll = 0;
                    var tagDirectoryInfo = new DirectoryInfo(tagDirectory);

                    foreach (var fileImage in tagDirectoryInfo.GetFiles())
                    {
                        using var image = Image.Load<Rgb24>(fileImage.FullName);
                        var tagPredictArr = NeuralNetworkWorker.NeuralNetworkResultKTopPercent(image, threadIndex.Value);

                        for (var k = 0; k < tagPredictArr.Length; k++)
                        {
                            if (tagOriginal == tagPredictArr[k])
                            {
                                if (k < countP10)
                                {
                                    countTrueP10++;
                                    if (k < countP5)
                                    {
                                        countTrueP5++;
                                        if (k < 1)
                                        {
                                            countTrueK1++;
                                        }
                                    }
                                }
                                break;
                            }
                        }

                        countAll++;
                    }

                    //
                    var vectAll = new Vector3(countAll / 100f);
                    var vectK = new Vector3(countTrueK1, countTrueP5, countTrueP10);
                    vectK = vectK / vectAll;

                    dataTable.Rows.Add(tagOriginal, countAll, vectK[0], vectK[1], vectK[2]);

                    count[0]++;
                }
            });

            worksheet.Cell(1, 1).InsertTable(dataTable);
            workbook.SaveAs("resultKTop.xlsx");
        }

        public sealed class PredictInfo
        {
            public int Id { get; set; }
            public int[] ResultArr { get; set; }

            public PredictInfo(int id, int size)
            {
                Id = id;
                ResultArr = new int[size];
            }
        }
        public static void GetAccuracyPredictClassesOriginal(int[] count)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet1");
            var dataTable = new DataTable();

            dataTable.Columns.Add("исходный \\ предсказаный", typeof(string));

            var predictInfoDict = new Dictionary<string, PredictInfo>();
            foreach (var label in NeuralNetworkWorker.Labels)
            {
                dataTable.Columns.Add(label, typeof(float));
                predictInfoDict.Add(label, new PredictInfo(dataTable.Columns.Count - 2, NeuralNetworkWorker.Labels.Length));
            }

            foreach (var label in NeuralNetworkWorker.Labels)
            {
                var row = dataTable.NewRow();
                row[0] = label;
                dataTable.Rows.Add(row);
            }

            int counter = 0;
            ThreadLocal<int> threadIndex = new ThreadLocal<int>(() => Interlocked.Increment(ref counter) - 1);

            Parallel.For(0, NeuralNetworkWorker.Labels.Length, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, i =>
            {
                var tagOriginal = NeuralNetworkWorker.Labels[i];

                var tagDirectory = Path.Combine(OriginalPath, tagOriginal);

                var tagDirectoryInfo = new DirectoryInfo(tagDirectory);

                foreach (var fileImage in tagDirectoryInfo.GetFiles())
                {
                    using var image = Image.Load<Rgb24>(fileImage.FullName);
                    var tagPredict = NeuralNetworkWorker.NeuralNetworkResult(image, threadIndex.Value);

                    predictInfoDict[tagPredict].ResultArr[predictInfoDict[tagOriginal].Id]++;
                }
                count[0]++;
            });

            for (var i = 0; i < NeuralNetworkWorker.Labels.Length; i++)
            {

                var tagPredict = NeuralNetworkWorker.Labels[i];
                var resultArr = predictInfoDict[tagPredict].ResultArr;
                var sum = resultArr.Sum() / 100f;
                if (sum != 0)
                {
                    var j = 0;
                    var vectSum = new Vector4(sum);
                    var countVectors = (NeuralNetworkWorker.Labels.Length / 4) * 4;
                    for (; j < countVectors; j += 4)
                    {
                        var vect = new Vector4(resultArr[j], resultArr[j + 1], resultArr[j + 2], resultArr[j + 3]);
                        vect /= vectSum;

                        var indexI = i + 1;
                        dataTable.Rows[j + 0][indexI] = vect[0];
                        dataTable.Rows[j + 1][indexI] = vect[1];
                        dataTable.Rows[j + 2][indexI] = vect[2];
                        dataTable.Rows[j + 3][indexI] = vect[3];
                    }

                    for (; j < NeuralNetworkWorker.Labels.Length; j++)
                    {
                        dataTable.Rows[j][i + 1] = resultArr[j] / sum;
                    }
                }
            }


            worksheet.Cell(1, 1).InsertTable(dataTable);
            workbook.SaveAs("resultPredict.xlsx");
        }
    }
}
