using System.Numerics;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace DataSet
{
    public static class DataSetImage
    {
        public static void Save(Image<Rgb24> image, string tags)
        {
            using var imageNew = ChangeResolution224x224(image);
            SaveFile(imageNew, tags);
        }

        public static Image<Rgb24> ChangeResolution224x224(Image<Rgb24> image)
        {
            return ChangeResolution(image, 224);
        }

        public static Image<Rgb24> ChangeResolution(Image<Rgb24> imageOriginal, int size)
        {
            var image = imageOriginal.Clone();
            if (image.Width != size || image.Height != size)
            {
                float maxImageSize = Math.Max(image.Width, image.Height);
                var scaleFactor = maxImageSize / size;
                int newWidth = (int)(image.Width / scaleFactor);
                int newHeight = (int)(image.Height / scaleFactor);

                image.Mutate(x => x.Resize(newWidth, newHeight));

                var resultImage = new Image<Rgb24>(size, size, new Rgb24(0, 0, 0));
                resultImage.Mutate(x => x.DrawImage(image, new Point(0, 0), 1f));

                image.Dispose();
                return resultImage;
            }
            return image;
        }

        private static readonly object lockObject = new();

        private static void SaveFile(Image<Rgb24> image, string tags)
        {
            var timestamp = DateTime.UtcNow;
            string pathDir = Path.Combine("DATA_SET", tags);
            Directory.CreateDirectory(pathDir);

            string path = Path.Combine(pathDir, timestamp.ToString("yyyy.MM.dd.HH.mm.ss.fff") + ".jpg");

            lock (lockObject)
            {
                while (File.Exists(path))
                {
                    Thread.Sleep(10);
                    timestamp = DateTime.UtcNow;
                    path = Path.Combine(pathDir, timestamp.ToString("yyyy.MM.dd.HH.mm.ss.fff") + ".jpg");
                }
                image.SaveAsJpeg(path);
            }
        }

        private const int SizeMax = 100;
        private const int SizeMin = SizeMax / 10;
        public static bool IsSimilarImage(Image<Rgb24> srcImage, Image<Rgb24> destImage)
        {
            using var resizedSrcImgMin = ChangeResolution(srcImage, SizeMin);
            using var resizedDestImgMin = ChangeResolution(destImage, SizeMin);
            if (Corral(resizedSrcImgMin, resizedDestImgMin) > 0.85f)
            {
                using var resizedSrcImgMax = ChangeResolution(srcImage, SizeMax);
                using var resizedDestImgMax = ChangeResolution(destImage, SizeMax);
                return Corral(resizedSrcImgMax, resizedDestImgMax) > 0.85f;
            }
            return false;
        }

        private static float Corral(Image<Rgb24> srcImage, Image<Rgb24> destImage)
        {
            var op = Vector3.Zero;
            var o1 = Vector3.Zero;
            var p1 = Vector3.Zero;
            var o2 = Vector3.Zero;
            var p2 = Vector3.Zero;

            for (int y = 0; y < srcImage.Height; y++)
            {
                for (int x = 0; x < srcImage.Width; x++)
                {
                    var pixel1 = srcImage[x, y];
                    var pixel2 = destImage[x, y];

                    var o = new Vector3(pixel1.R, pixel1.G, pixel1.B);
                    var p = new Vector3(pixel2.R, pixel2.G, pixel2.B);

                    op += o * p;
                    o1 += o;
                    p1 += p;
                    o2 += o * o;
                    p2 += p * p;
                }
            }

            var n = srcImage.Height * srcImage.Width;
            var up = n * op - o1 * p1;
            var down = Vector3.SquareRoot((n * o2 - o1 * o1) * (n * p2 - p1 * p1));
            var resultVect = up / down;
            return (resultVect.X + resultVect.Y + resultVect.Z) / 3.0f;
        }
    }
}