using ClosedXML.Excel;

using DataSet;

using DocumentFormat.OpenXml.Wordprocessing;

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
                    Directory.Delete(tag, true);
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

        public static void GetAccuracyClassesOriginal(int[] count)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet1");

            var countP5 = int.Max((int)(NeuralNetworkWorker.Labels.Length * 0.05), 1);
            var countP10 = int.Max((int)(NeuralNetworkWorker.Labels.Length * 0.10), 1);

            worksheet.Cell(1, 1).Value = "Тег";
            worksheet.Cell(1, 2).Value = "Количество объектов";
            worksheet.Cell(1, 3).Value = "Точность k=1";
            worksheet.Cell(1, 4).Value = $"Точность 5% k={countP5}";
            worksheet.Cell(1, 5).Value = $"Точность 10% k={countP10}";

            var tagsDirectory = Path.Combine(MAIN_DIRECTORY, ORIGINAL_PATH);

            for (var i = 0; i <NeuralNetworkWorker.Labels.Length; i++)
            {
                var tagOriginal = NeuralNetworkWorker.Labels[i];
                try
                {
                    var tagDirectory = Path.Combine(tagsDirectory, tagOriginal);

                    var countTrueK1 = 0;
                    var countTrueP5 = 0;
                    var countTrueP10 = 0;
                    var countAll = 0;
                    var tagDirectoryInfo = new DirectoryInfo(tagDirectory);

                    foreach (var fileImage in tagDirectoryInfo.GetFiles())
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
                    }

                    worksheet.Cell(i + 2, 1).Value = tagOriginal;
                    worksheet.Cell(i + 2, 2).Value = countAll;
                    worksheet.Cell(i + 2, 3).Value = (countTrueK1 * 100f) / countAll;
                    worksheet.Cell(i + 2, 4).Value = (countTrueP5 * 100f) / countAll;
                    worksheet.Cell(i + 2, 5).Value = (countTrueP10 * 100f) / countAll;
                }
                catch 
                {
                    worksheet.Cell(i + 2, 1).Value = tagOriginal;
                    worksheet.Cell(i + 2, 2).Value = 0;
                    worksheet.Cell(i + 2, 3).Value = 0;
                    worksheet.Cell(i + 2, 4).Value = 0;
                    worksheet.Cell(i + 2, 5).Value = 0;
                }

                count[0]++;
            }

            workbook.SaveAs("result.xlsx");
        }
    }
}
