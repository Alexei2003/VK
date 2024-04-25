using DataSet;
using System.Drawing;
using System.Drawing.Imaging;

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
                        src.MoveTo(destination + "\\" + src.Name);
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

        public static void MoveDataFromNewToReady()
        {
            var tagDirectories = Directory.GetDirectories(MAIN_DIRECTORY + "\\" + NEW_PATH);

            Parallel.ForEach(tagDirectories, tag =>
            {
                DirectoryMove(tag, MAIN_DIRECTORY + "\\" + ORIGINAL_PATH + "\\" + Path.GetDirectoryName(tag), true);
            });
        }

        public static void MoveDataToOutput()
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
                new(){RotateAngle = 90, Reflection = new GeneratorArtificialImage.GeneratorArtificialImageSetting.ReflectionStruct(){ X = true } },
            };

            Parallel.ForEach(tagDirectories, tag =>
            {
                var tagDirectoryFiles = Directory.GetFiles(tag);
                foreach (var imagePath in tagDirectoryFiles)
                {
                    var originalImage = new Bitmap(imagePath);
                    var countImage = ((max / tagDirectoryFiles.Count()) % settings.Length) + 1;
                    var directoryPath = MAIN_DIRECTORY + "\\" + OUTPUT_PATH + "\\" + GetDirectoryName(tag);
                    var imageName = Path.GetFileNameWithoutExtension(imagePath);

                    Directory.CreateDirectory(directoryPath);

                    var outputFilesReady = Directory.GetFiles(directoryPath);

                    if(!outputFilesReady.Contains(directoryPath + "\\" + imageName + ".jpg"))
                    {
                        originalImage.Save(directoryPath + "\\" + imageName + ".jpg", ImageFormat.Jpeg);
                    }

                    var outputImages = GeneratorArtificialImage.Generate(originalImage, settings, countImage);

                    for (int i = 0; i < outputImages.Count; i++)
                    {
                        var settingAdd = "-" + i.ToString();
                        if (!outputFilesReady.Contains(directoryPath + "\\" + imageName + settingAdd + ".jpg"))
                        {
                            outputImages[i].Save(directoryPath + "\\" + imageName + settingAdd + ".jpg", ImageFormat.Jpeg);
                        }
                    }
                }
            });

        }
    }
}
