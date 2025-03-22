using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Numerics;

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

        private static Image<Rgb24> ChangeResolution(Image<Rgb24> imageOriginal, int maxSize)
        {
            var image = imageOriginal.Clone();
            if (image.Width != maxSize || image.Height != maxSize)
            {
                float maxImageSize = Math.Max(image.Width, image.Height);
                var scaleFactor = maxImageSize / maxSize;
                int newWidth = (int)(image.Width / scaleFactor);
                int newHeight = (int)(image.Height / scaleFactor);

                image.Mutate(x => x.Resize(newWidth, newHeight));

                var resultImage = new Image<Rgb24>(maxSize, maxSize, new Rgb24(0, 0, 0));
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

        private const int maxSize = 100;
        public static bool IsSimilarImage(Image<Rgb24> img1, Image<Rgb24> img2)
        {
            using var resizedImg1 = ChangeResolution(img1, maxSize);
            using var resizedImg2 = ChangeResolution(img2, maxSize);
            return Corral(resizedImg1, resizedImg2) > 0.85f;
        }

        private static float Corral(Image<Rgb24> img1, Image<Rgb24> img2)
        {
            Vector3 op = Vector3.Zero, o1 = Vector3.Zero, p1 = Vector3.Zero, o2 = Vector3.Zero, p2 = Vector3.Zero;
            int n = 0;

            for (int y = 0; y < Math.Min(img1.Height, img2.Height); y++)
            {
                for (int x = 0; x < Math.Min(img1.Width, img2.Width); x++)
                {
                    var pixel1 = img1[x, y];
                    var pixel2 = img2[x, y];

                    var o = new Vector3(pixel1.R, pixel1.G, pixel1.B);
                    var p = new Vector3(pixel2.R, pixel2.G, pixel2.B);

                    op += o * p;
                    o1 += o;
                    p1 += p;
                    o2 += o * o;
                    p2 += p * p;

                    n++;
                }
            }

            var up = n * op - o1 * p1;
            var down = Vector3.SquareRoot((n * o2 - o1 * o1) * (n * p2 - p1 * p1));
            var resultVect = up / down;
            return (resultVect.X + resultVect.Y + resultVect.Z) / 3.0f;
        }
    }
}
