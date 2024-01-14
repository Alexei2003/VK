using System.Drawing.Imaging;

namespace AddPost.Classes.DataSet
{
    internal class DataSetPhoto
    {
        private const double MAX_SIZE = 224;

        public static void Add(Bitmap image, string tags)
        {
            image = ChangeResolution(image);

            Save(image, tags);
        }

        public static Bitmap ChangeResolution(Bitmap image)
        {
            if (image.Width - image.Height > 0)
            {
                if (image.Width > MAX_SIZE)
                {
                    var delta = image.Width / MAX_SIZE;
                    return new Bitmap(image, Convert.ToInt32(image.Width / delta), Convert.ToInt32(image.Height / delta));
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
                    return new Bitmap(image, Convert.ToInt32(image.Width / delta), Convert.ToInt32(image.Height / delta));
                }
                else
                {
                    return image;
                }
            }
        }

        private static void Save(Bitmap image, string tags)
        {
            var data = DateTime.Now;
            data.AddHours(3);

            string path = "DATA_SET\\" + tags;

            Directory.CreateDirectory(path);

            path = path + "\\" + data.ToString("yyyy.MM.dd.HH.mm.ss.fff") + ".jpg";

            if(File.Exists(path))
            {
                Thread.Sleep(10);
                data = DateTime.Now;
                data.AddHours(3);
                path = path + "\\" + data.ToString("yyyy.MM.dd.HH.mm.ss.fff") + ".jpg";
            }
            image.Save(path, ImageFormat.Jpeg);
        }
    }
}
