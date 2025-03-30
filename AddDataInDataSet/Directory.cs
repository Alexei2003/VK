using System.Numerics;

using ClosedXML.Excel;

using DataSet;

using NeuralNetwork;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace AddDataInDataSet
{
    internal static class WorkWithDirectory
    {
        private const string MAIN_DIRECTORY = "D:\\NeuralNetwork\\DataSet\\ARTS";
        private const string NEW_PATH = $"New";
        private const string ORIGINAL_PATH = $"Original";
        private const string SMALL_PATH = $"Small";

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
            }
            if (deleteOriginal)
            {
                Directory.Delete(source, true);
            }
        }

        private static bool Similar(FileInfo src, string destination)
        {
            using var srcImage = Image.Load<Rgb24>(src.FullName);
            var destinationInfo = new DirectoryInfo(destination);
            foreach (var dest in destinationInfo.GetFiles())
            {
                using var destImage = Image.Load<Rgb24>(dest.FullName);
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
            var tagDirectories = Directory.GetDirectories(Path.Combine(MAIN_DIRECTORY, NEW_PATH));

            Parallel.ForEach(tagDirectories, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, tag =>
            {
                DirectoryMove(tag, Path.Combine(MAIN_DIRECTORY, ORIGINAL_PATH, GetDirectoryName(tag)), checkSimilar: true, deleteOriginal: true, changeResolution: true);

                count[0]++;
            });
        }

        public static void FixDataInOriginal(int[] count)
        {
            var tagDirectories = Directory.GetDirectories(Path.Combine(MAIN_DIRECTORY, ORIGINAL_PATH));

            var tagList = new TagsList();

            Parallel.ForEach(tagDirectories, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, tag =>
            {
                var name = GetDirectoryName(tag);

                if (!tagList.ContainTag(name))
                {
                    Console.WriteLine(tag);
                }
                else
                {
                    var sourceInfo = new DirectoryInfo(tag);
                    if (sourceInfo.GetFiles().Length < 100)
                    {
                        DirectoryMove(tag, Path.Combine(MAIN_DIRECTORY, SMALL_PATH, name), deleteOriginal: true);
                    }
                }

                count[0]++;
            });
        }

        public static void GetAccuracyKTopClassesOriginal(int[] count)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet1");
            var table = new object[NeuralNetworkWorker.Labels.Length, 5];

            var countP5 = int.Max((int)(NeuralNetworkWorker.Labels.Length * 0.05), 1);
            var countP10 = int.Max((int)(NeuralNetworkWorker.Labels.Length * 0.10), 1);
            var tagsDirectory = Path.Combine(MAIN_DIRECTORY, ORIGINAL_PATH);

            table[0, 0] = "Тег";
            table[0, 1] = "Количество объектов";
            table[0, 2] = "Точность k=1";
            table[0, 3] = $"Точность 5% k={countP5}";
            table[0, 4] = $"Точность 10% k={countP10}";

            for (var i = 0; i < NeuralNetworkWorker.Labels.Length; i++)
            {
                var tagOriginal = NeuralNetworkWorker.Labels[i];

                var tagDirectory = Path.Combine(tagsDirectory, tagOriginal);

                var countTrueK1 = 0;
                var countTrueP5 = 0;
                var countTrueP10 = 0;
                var countAll = 0;
                var tagDirectoryInfo = new DirectoryInfo(tagDirectory);

                Parallel.ForEach(tagDirectoryInfo.GetFiles(), fileImage =>
                {
                    using var image = Image.Load<Rgb24>(fileImage.FullName);
                    var tagPredictArr = NeuralNetworkWorker.NeuralNetworkResultKTopPercent(image);

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
                });

                //
                var vectAll = new Vector3(countAll / 100f);
                var vectK = new Vector3(countTrueK1, countTrueP5, countTrueP10);
                vectK = vectK / vectAll;

                var index = i + 1;
                table[index, 0] = tagOriginal;
                table[index, 1] = countAll;
                table[index, 2] = vectK[0];
                table[index, 3] = vectK[1];
                table[index, 4] = vectK[2];

                count[0]++;
            }

            worksheet.Cell(1, 1).InsertData(table);
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
            var table = new object[NeuralNetworkWorker.Labels.Length + 1, NeuralNetworkWorker.Labels.Length + 1];

            table[0, 0] = "исходный \\ предсказаный";

            var predictInfoDict = new Dictionary<string, PredictInfo>();
            for (var i = 0; i < NeuralNetworkWorker.Labels.Length; i++)
            {
                var index = i + 1;
                table[index, 0] = NeuralNetworkWorker.Labels[i];
                table[0, index] = NeuralNetworkWorker.Labels[i];
                predictInfoDict.Add(NeuralNetworkWorker.Labels[i], new PredictInfo(i, NeuralNetworkWorker.Labels.Length));
            }

            var tagsDirectory = Path.Combine(MAIN_DIRECTORY, ORIGINAL_PATH);
            for (var i = 0; i < NeuralNetworkWorker.Labels.Length; i++)
            {
                var tagOriginal = NeuralNetworkWorker.Labels[i];

                var tagDirectory = Path.Combine(tagsDirectory, tagOriginal);

                var tagDirectoryInfo = new DirectoryInfo(tagDirectory);

                Parallel.ForEach(tagDirectoryInfo.GetFiles(), fileImage =>
                {
                    using var image = Image.Load<Rgb24>(fileImage.FullName);
                    var tagPredict = NeuralNetworkWorker.NeuralNetworkResult(image);

                    predictInfoDict[tagPredict].ResultArr[predictInfoDict[tagOriginal].Id]++;
                });
                count[0]++;
            }

            Parallel.For(0, NeuralNetworkWorker.Labels.Length, i =>
            {

                var tagPredict = NeuralNetworkWorker.Labels[i];
                var resultArr = predictInfoDict[tagPredict].ResultArr;
                var sum = resultArr.Sum() / 100f;
                var j = 0;
                var vectSum = new Vector4(sum);
                var countVectors = (NeuralNetworkWorker.Labels.Length / 4) * 4;
                for (; j < countVectors; j += 4)
                {
                    var vect = new Vector4(resultArr[j], resultArr[j + 1], resultArr[j + 2], resultArr[j + 3]);
                    vect /= vectSum;

                    var indexJ = 1 + j;
                    var indexI = 1 + i;
                    table[indexJ++, indexI] = vect[0];
                    table[indexJ++, indexI] = vect[1];
                    table[indexJ++, indexI] = vect[2];
                    table[indexJ++, indexI] = vect[3];
                }

                for (; j < NeuralNetworkWorker.Labels.Length; j++)
                {
                    table[j + 1, i + 1] = resultArr[j] / sum;
                }
            });


            worksheet.Cell(1, 1).InsertData(table);
            workbook.SaveAs("resultPredict.xlsx");
        }
    }
}
