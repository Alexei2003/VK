﻿using ClosedXML.Excel;
using DataSet;
using System.Drawing;

namespace AddDataInDataSet
{
    internal static class WorkWithDirectory
    {
        private const string MAIN_DIRECTORY = "D:\\NeuralNetwork\\DataSet\\ARTS";
        private const string NEW_PATH = $"New";
        private const string ORIGINAL_PATH = $"Original";

        private static void DirectoryMove(string source, string destination, bool checkSimilar, bool deleteOriginal = false)
        {
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            var sourceInfo = new DirectoryInfo(source);
            foreach (var src in sourceInfo.GetFiles())
            {
                var similar = false;
                if (checkSimilar)
                {
                    using var srcBmp = new Bitmap(src.FullName);
                    var destinationInfo = new DirectoryInfo(destination);
                    foreach (var dest in destinationInfo.GetFiles())
                    {
                        using var destBmp = new Bitmap(dest.FullName);
                        if (DataSetImage.IsSimilarImage(srcBmp, destBmp))
                        {
                            similar = true;
                            break;
                        }
                    }
                }
                if (!similar && !File.Exists(Path.Combine(destination, src.Name)))
                {
                    src.MoveTo(Path.Combine(destination, src.Name));
                }
            }
            if (deleteOriginal)
            {
                Directory.Delete(source, true);
            }
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
                DirectoryMove(tag, Path.Combine(MAIN_DIRECTORY, ORIGINAL_PATH, GetDirectoryName(tag)), true, true);

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

                if (tagList.ContainTag(name))
                {
                    if (name == "#Original")
                    {
                        var tageInfo = new DirectoryInfo(tag);

                        var files = tageInfo.GetFiles();

                        var pathBase = Path.Combine(MAIN_DIRECTORY, ORIGINAL_PATH, "#Original_");
                        while (files.Length >= 500)
                        {
                            var pathNewOriginal = pathBase + DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss.fff");
                            if (Directory.Exists(pathNewOriginal))
                            {
                                Thread.Sleep(100);
                                continue;
                            }

                            Directory.CreateDirectory(pathNewOriginal);
                            for (int i = 0; i < 500; i++)
                            {
                                files[i].MoveTo(Path.Combine(pathNewOriginal, files[i].Name));
                            }

                            files = tageInfo.GetFiles();
                        }
                    }
                }
                else
                {
                    if (!name.Contains("#Original_"))
                    {
                        Directory.Delete(tag, true);
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
                var tagOriginal = GetDirectoryName(tagDirectories[i]);

                int countTrue = 0;
                int countAll = 0;
                var tagDirectoryInfo = new DirectoryInfo(tagDirectories[i]);
                foreach (var fileImage in tagDirectoryInfo.GetFiles())
                {
                    var image = new Bitmap(fileImage.FullName);

                    var tagPredict = NeuralNetwork.NeuralNetwork.NeuralNetworkResult(image);

                    if (tagOriginal == tagPredict)
                    {
                        countTrue++;
                    }

                    countAll++;
                }

                worksheet.Cell(i + 2, 1).Value = tagOriginal;
                worksheet.Cell(i + 2, 2).Value = (countTrue * 100f) / countAll;
                worksheet.Cell(i + 2, 3).Value = countAll;

                count[0]++;
            }

            workbook.SaveAs("result.xlsx");
        }
    }
}
