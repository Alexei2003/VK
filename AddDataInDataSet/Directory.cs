﻿using ClosedXML.Excel;
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
                        using var bmpOriginal = Image.Load<Rgb24>(src.FullName);
                        using var bmp = DataSetImage.ChangeResolution224x224(bmpOriginal);
                        bmp.SaveAsJpeg(Path.Combine(destination, src.Name));
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
            using var srcBmp = Image.Load<Rgb24>(src.FullName);
            var destinationInfo = new DirectoryInfo(destination);
            foreach (var dest in destinationInfo.GetFiles())
            {
                using var destBmp = Image.Load<Rgb24>(dest.FullName);
                if (DataSetImage.IsSimilarImage(srcBmp, destBmp))
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
            var tagDirectories = Directory.GetDirectories(Path.Combine(MAIN_DIRECTORY, ORIGINAL_PATH));

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet1");

            worksheet.Cell(1, 1).Value = "Тег";
            worksheet.Cell(1, 2).Value = "Точность (%)";
            worksheet.Cell(1, 3).Value = "Количество объектов";

            for (var i = 0; i < tagDirectories.Length; i++)
            {
                var tagDirectory = tagDirectories[i];
                var tagOriginal = GetDirectoryName(tagDirectory);

                int countTrue = 0;
                int countAll = 0;
                var tagDirectoryInfo = new DirectoryInfo(tagDirectory);

                foreach (var fileImage in tagDirectoryInfo.GetFiles())
                {
                    using var bmp = Image.Load<Rgb24>(fileImage.FullName);
                    var tagPredict = NeuralNetwork.NeuralNetwork.NeuralNetworkResult(bmp);

                    if (tagOriginal == tagPredict)
                    {
                        countTrue++;
                    }

                    countAll++;
                }

                // Запись результатов в ячейки
                worksheet.Cell(i + 2, 1).Value = tagOriginal;
                worksheet.Cell(i + 2, 2).Value = (countTrue * 100f) / countAll;
                worksheet.Cell(i + 2, 3).Value = countAll;

                count[0]++;
            }


            workbook.SaveAs("result.xlsx");
        }
    }
}
