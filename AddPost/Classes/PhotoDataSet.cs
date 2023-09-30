using System.Drawing.Imaging;

namespace AddPost.Classes
{
    internal class PhotoDataSet
    {
        private const double MAX_SIZE = 1024;

        public static void Add(Bitmap image, string tags)
        {
            if (image.Width - image.Height > 0)
            {
                if (image.Width > MAX_SIZE)
                {
                    var delta = image.Width / MAX_SIZE;
                    image = ChangeResolution(image, delta);
                }
            }
            else
            {
                if (image.Height > MAX_SIZE)
                {
                    var delta = image.Height / MAX_SIZE;
                    image = ChangeResolution(image, delta);
                }
            }

            Save(image, tags);
        }

        private static Bitmap ChangeResolution(Bitmap originalImage, double delta)
        {
            var image = new Bitmap(originalImage, Convert.ToInt32(originalImage.Width / delta), Convert.ToInt32(originalImage.Height / delta));
            return image;
        }

        private static void Save(Bitmap image, string tags)
        {
            var data = DateTime.Now;
            data.AddHours(3);

            string path = "DATA_SET\\" + tags;

            Directory.CreateDirectory(path);

            image.Save(path + "\\" + data.ToString("yyyy.MM.dd.HH.mm.ss") + ".jpg", ImageFormat.Jpeg);
        }
    }
}
