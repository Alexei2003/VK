using System.Drawing.Imaging;

namespace AddPost.Classes
{
    internal class PhotoDataSet
    {
        private const double MAX_SIZE = 224;

        public static void Add(Bitmap image, string tags)
        {
            ChangeResolution(image);

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

            image.Save(path + "\\" + data.ToString("yyyy.MM.dd.HH.mm.ss") + ".jpg", ImageFormat.Jpeg);
        }
    }
}
