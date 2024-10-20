﻿using DataSet;
using System.Drawing;
using System.Drawing.Imaging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AddDataInDataSet
{
    internal static class WorkWithDirectory
    {
        private const string MAIN_DIRECTORY = "ARTS";
        private const string NEW_PATH = $"New";
        private const string ORIGINAL_PATH = $"Original";
        private const string OUTPUT_PATH = $"Output";

        private static void DirectoryMove(string source, string destination, bool checkSimilar, bool deleteOriginal = false)
        {
            if (Directory.Exists(destination))
            {
                var sourceInfo = new DirectoryInfo(source);
                var destinationInfo = new DirectoryInfo(destination);

                foreach (var src in sourceInfo.GetFiles())
                {
                    var similar = false;
                    if (checkSimilar)
                    {
                        using var srcBmp = new Bitmap(src.FullName);
                        foreach (var dest in destinationInfo.GetFiles())
                        {
                            using var destBmp = new Bitmap(dest.FullName);
                            if (DataSetPhoto.IsSimilarPhoto(srcBmp, destBmp))
                            {
                                similar = true;
                                break;
                            }
                        }
                    }
                    if (!similar)
                    {
                        if (!File.Exists(destination + "\\" + src.Name))
                        {
                            src.MoveTo(destination + "\\" + src.Name);
                        }
                    }
                }
                if (deleteOriginal)
                {
                    Directory.Delete(source, true);
                }
            }
            else
            {
                Directory.Move(source, destination);
            }
        }

        private static string GetDirectoryName(string dir)
        {
            return dir.Split('\\', StringSplitOptions.RemoveEmptyEntries).Last();
        }

        public static void MoveDataFromNewToOriginal(int[] count)
        {
            var tagDirectories = Directory.GetDirectories(MAIN_DIRECTORY + "\\" + NEW_PATH);

            Parallel.ForEach(tagDirectories, tag =>
            {
                count[0]++;

                DirectoryMove(tag, MAIN_DIRECTORY + "\\" + ORIGINAL_PATH + "\\" + GetDirectoryName(tag), true, true);
            });
        }

        public static void MoveDataToOutput(int[] count)
        {
            var tagDirectories = Directory.GetDirectories(MAIN_DIRECTORY + "\\" + ORIGINAL_PATH);

            int max = 0;

            Parallel.ForEach(tagDirectories, tag =>
            {
                var tmpFiles = Directory.GetFiles(tag);
                if (tmpFiles.Length > max)
                {
                    max = tmpFiles.Length;
                }
            });

            var settings = new GeneratorArtificialImage.GeneratorArtificialImageSetting[]
            {
                new(){RotateAngle = 180, ContrastCorrection = 20, AdditiveNoise = true, Resize = 168},
                new(){RotateAngle = 90, Reflection = new GeneratorArtificialImage.GeneratorArtificialImageSetting.ReflectionStruct(){ X = true }, ContrastCorrection = 10, GaussianBlur = true, Resize = 196},
                new(){Reflection = new GeneratorArtificialImage.GeneratorArtificialImageSetting.ReflectionStruct(){ Y = true}, ContrastCorrection = -50 },
                new(){RotateAngle = 270, GaussianBlur = true, ContrastCorrection = -25 },
            };

            var settingAdds = new string[settings.Length];

            for (var i = 0; i < settingAdds.Length; i++)
            {
                settingAdds[i] = settings[i].GetCodeAction();
            }

            Parallel.ForEach(tagDirectories, tag =>
            {
                count[0]++;

                var tagDirectoryFiles = Directory.GetFiles(tag);
                foreach (var imagePath in tagDirectoryFiles)
                {
                    var originalImage = new Bitmap(imagePath);
                    double countImageExtraDouble = double.Round(((max * 1.0) / tagDirectoryFiles.Count()) - 1);
                    int countImageExtra = Convert.ToInt32(countImageExtraDouble);

                    if (countImageExtra > settings.Length)
                    {
                        countImageExtra = settings.Length;
                    }

                    var directoryPath = MAIN_DIRECTORY + "\\" + OUTPUT_PATH + "\\" + GetDirectoryName(tag);
                    var imageName = Path.GetFileNameWithoutExtension(imagePath);

                    Directory.CreateDirectory(directoryPath);

                    var outputFilesReady = Directory.GetFiles(directoryPath);

                    if (!outputFilesReady.Contains(directoryPath + "\\" + imageName + ".jpg"))
                    {
                        originalImage.Save(directoryPath + "\\" + imageName + ".jpg", ImageFormat.Jpeg);
                    }

                    var outputImages = GeneratorArtificialImage.Generate(originalImage, settings, countImageExtra);

                    for (int i = 0; i < outputImages.Count; i++)
                    {
                        if (!outputFilesReady.Contains(directoryPath + "\\" + imageName + settingAdds[i] + ".jpg"))
                        {
                            outputImages[i].Save(directoryPath + "\\" + imageName + settingAdds[i] + ".jpg", ImageFormat.Jpeg);
                        }
                    }
                }
            });

        }

        public static void FixDataInOriginal(int[] count)
        {
            var tagDirectories = Directory.GetDirectories(MAIN_DIRECTORY + "\\" + ORIGINAL_PATH);

            var tagList = new TagsList();

            Parallel.ForEach(tagDirectories, tag =>
            {
                count[0]++;

                var name = GetDirectoryName(tag);

                if (tagList.ContainTag(name)) 
                { 
                    if(name == "#Original")
                    {
                        var tageInfo = new DirectoryInfo(tag);

                        var files = tageInfo.GetFiles();

                        var pathBase = MAIN_DIRECTORY + "\\" + ORIGINAL_PATH + "\\" + "#Original_";
                        while (files.Length >= 500)
                        {
                            var pathNewOriginal = pathBase + DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss.fff");
                            if (Directory.Exists(pathNewOriginal))
                            {
                                Thread.Sleep(100);
                                continue;
                            }

                            Directory.CreateDirectory(pathNewOriginal);
                            string pathOld;
                            for (int i = 0; i < 500; i++) 
                            {
                                pathOld = files[i].FullName;
                                files[i].MoveTo(pathNewOriginal + "\\" + files[i].Name);
                                File.Delete(pathOld);
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
            });
        }
    }
}
