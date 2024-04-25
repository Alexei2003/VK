using AForge.Imaging;
using System.Drawing;
using System.Drawing.Imaging;

namespace DataSet
{
    public static class DataSetPhoto
    {
        private const double MAX_SIZE = 224;

        public static void Save(Bitmap image, string tags)
        {
            image = ChangeResolution224224(image);

            SaveFile(image, tags);
        }

        public static Bitmap ChangeResolution224224(Bitmap image)
        {
            if (image.Width - image.Height > 0)
            {
                if (image.Width > MAX_SIZE)
                {
                    var delta = image.Width / MAX_SIZE;
                    return ImageTo24bpp(new Bitmap(image, Convert.ToInt32(image.Width / delta), Convert.ToInt32(image.Height / delta)));
                }
                else
                {
                    return image;
                }
            }
            else
            {
                if (image.Height > MAX_SIZE)
                {
                    var delta = image.Height / MAX_SIZE;
                    return ImageTo24bpp(new Bitmap(image, Convert.ToInt32(image.Width / delta), Convert.ToInt32(image.Height / delta)));
                }
                else
                {
                    return image;
                }
            }
        }
        public static Bitmap ImageTo24bpp(Bitmap img)
        {
            var bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format24bppRgb);
            using (var gr = Graphics.FromImage(bmp))
            {
                gr.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height));
            }
            return bmp;
        }

        public static Bitmap ImageTo32bpp(Bitmap img)
        {
            var bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb);
            using (var gr = Graphics.FromImage(bmp))
            {
                gr.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height));
            }
            return bmp;
        }

        private static void SaveFile(Bitmap image, string tags)
        {
            var data = DateTime.Now;
            data.AddHours(3);

            string path = "DATA_SET\\" + tags;

            Directory.CreateDirectory(path);

            path = path + "\\" + data.ToString("yyyy.MM.dd.HH.mm.ss.fff") + ".jpg";

            if (File.Exists(path))
            {
                Thread.Sleep(10);
                data = DateTime.Now;
                data.AddHours(3);
                path = path + "\\" + data.ToString("yyyy.MM.dd.HH.mm.ss.fff") + ".jpg";
            }
            image.Save(path, ImageFormat.Jpeg);
        }

        public static bool IsSimilarPhoto(Bitmap bmp1, Bitmap bmp2)
        {
            // Создаем экземпляр алгоритма сравнения шаблонов
            var tm = new ExhaustiveTemplateMatching(0.8f);

            // Находим все совпадения с заданным порогом сходства
            var matches = tm.ProcessImage(ImageTo24bpp(new Bitmap(bmp1, bmp2.Width, bmp2.Height)), bmp2);

            if (matches.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
