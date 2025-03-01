using ClosedXML.Excel;
using DataSet;
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
                    if (sourceInfo.GetFiles().Length < 50)
                    {
                        DirectoryMove(tag, Path.Combine(MAIN_DIRECTORY, SMALL_PATH, name), deleteOriginal: true);
                    }
                }

                count[0]++;
            });
        }

        public static void GetAccuracyClassesOriginal(int[] count)
        {
            var tagDirectories = Directory.GetDirectories(Path.Combine(MAIN_DIRECTORY, ORIGINAL_PATH));

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet1");

            worksheet.Cell(1, 1).Value = "Тег";
            worksheet.Cell(1, 2).Value = "Количество объектов";
            worksheet.Cell(1, 3).Value = "Точность k=1  (%)";
            worksheet.Cell(1, 4).Value = "Точность k=5  (%)";
            worksheet.Cell(1, 5).Value = "Точность k=10 (%)";
            worksheet.Cell(1, 6).Value = "Точность k=15 (%)";

            for (var i = 0; i < tagDirectories.Length; i++)
            {
                var tagDirectory = tagDirectories[i];
                var tagOriginal = GetDirectoryName(tagDirectory);

                var countTrueK1 = 0;
                var countTrueK5 = 0;
                var countTrueK10 = 0;
                var countTrueK15 = 0;
                var countAll = 0;
                var tagDirectoryInfo = new DirectoryInfo(tagDirectory);

                foreach (var fileImage in tagDirectoryInfo.GetFiles())
                {
                    using var image = Image.Load<Rgb24>(fileImage.FullName);
                    var tagPredictArr = NeuralNetwork.NeuralNetwork.NeuralNetworkResultKTop(image);

                    for(var k = 0; k< tagPredictArr.Length; k++)
                    {
                        if (tagOriginal == tagPredictArr[k])
                        {
                            if (k < 15)
                            {
                                countTrueK15++;
                                if (k < 10)
                                {
                                    countTrueK10++;
                                    if (k < 5)
                                    {
                                        countTrueK5++; 
                                        if (k < 1)
                                        {
                                            countTrueK1++;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    countAll++;
                }

                // Запись результатов в ячейки
                worksheet.Cell(i + 2, 1).Value = tagOriginal;
                worksheet.Cell(i + 2, 2).Value = countAll;
                worksheet.Cell(i + 2, 3).Value = (countTrueK1  * 100f) / countAll;
                worksheet.Cell(i + 2, 4).Value = (countTrueK5  * 100f) / countAll;
                worksheet.Cell(i + 2, 5).Value = (countTrueK10 * 100f) / countAll;
                worksheet.Cell(i + 2, 6).Value = (countTrueK15 * 100f) / countAll;

                count[0]++;
            }


            workbook.SaveAs("result.xlsx");
        }
    }
}
