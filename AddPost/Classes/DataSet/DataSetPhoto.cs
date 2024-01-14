using System.Drawing.Imaging;

namespace AddPost.Classes.DataSet
{
    internal static class DataSetPhoto
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
            if (bmp1.Width == bmp2.Width && bmp1.Height == bmp2.Height)
            {
                /*                Rectangle rect = new Rectangle(0, 0, bmp1.Width, bmp1.Height);
                                BitmapData bmp1Data = bmp1.LockBits(rect, ImageLockMode.ReadWrite, bmp1.PixelFormat);
                                BitmapData bmp2Data = bmp2.LockBits(rect, ImageLockMode.ReadWrite, bmp2.PixelFormat);


                                // Получаем адрес первой строки.
                                IntPtr ptr1 = bmp1Data.Scan0;

                                // Объявляем массив для хранения байтов изображения.
                                int bytes1 = Math.Abs(bmp1Data.Stride) * bmp1.Height;
                                byte[] rgbValues1 = new byte[bytes1];

                                // Копируем RGB значения в массив.
                                System.Runtime.InteropServices.Marshal.Copy(ptr1, rgbValues1, 0, bytes1);



                                // Получаем адрес первой строки.
                                IntPtr ptr2 = bmp2Data.Scan0;

                                // Объявляем массив для хранения байтов изображения.
                                int bytes2 = Math.Abs(bmp2Data.Stride) * bmp1.Height;
                                byte[] rgbValues2 = new byte[bytes2];

                                // Копируем RGB значения в массив.
                                System.Runtime.InteropServices.Marshal.Copy(ptr2, rgbValues2, 0, bytes2);

                                int count = 0;
                                int similar = 0;

                                int stride;
                                if(Math.Abs(bmp1Data.Stride) > Math.Abs(bmp2Data.Stride))
                                {
                                    stride = Math.Abs(bmp2Data.Stride);
                                }
                                else
                                {
                                    stride = Math.Abs(bmp1Data.Stride);
                                }

                                for (int height = 0; height < bmp1.Height; height += 20)
                                {             
                                    for (int width = 0; width < stride; width += 5)
                                    {
                                        count++;
                                        var index = width * height;
                                        if (rgbValues1[index] == rgbValues2[index])
                                        {
                                            similar++;
                                        }
                                    }
                                }

                                bmp1.UnlockBits(bmp1Data);
                                bmp2.UnlockBits(bmp2Data);

                                if (similar/(count*1.0) > 0.1)
                                {
                                    return true;
                                }*/

                return true;
            }
            return false;

        }
    }
}
