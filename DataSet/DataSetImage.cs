using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;

namespace DataSet
{
    public static class DataSetImage
    {
        public static void Save(Bitmap bmp, string tags)
        {
            bmp = ImageTo32bpp(ChangeResolution224x224(bmp));

            SaveFile(bmp, tags);
        }

        public static Bitmap ChangeResolution224x224(Bitmap bmp)
        {
            return ChangeResolution(bmp, 224);
        }

        public static Bitmap ChangeResolution(Bitmap bmp, float maxSize)
        {
            if (bmp.Width > bmp.Height)
            {
                if (bmp.Width > maxSize)
                {
                    var delta = bmp.Width / maxSize;
                    return new Bitmap(bmp, Convert.ToInt32(bmp.Width / delta), Convert.ToInt32(bmp.Height / delta));
                }
                else
                {
                    return bmp;
                }
            }
            else
            {
                if (bmp.Height > maxSize)
                {
                    var delta = bmp.Height / maxSize;
                    return new Bitmap(bmp, Convert.ToInt32(bmp.Width / delta), Convert.ToInt32(bmp.Height / delta));
                }
                else
                {
                    return bmp;
                }
            }
        }
        public static Bitmap ImageTo32bpp(Bitmap bmp)
        {
            return bmp.Clone(new Rectangle { X = 0, Y = 0, Width = bmp.Width, Height = bmp.Height }, PixelFormat.Format32bppRgb);
        }

        private static void SaveFile(Bitmap bmp, string tags)
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
            bmp.Save(path, ImageFormat.Jpeg);
        }

        private const float maxSize = 100f;
        public static bool IsSimilarImage(Bitmap bmp1, Bitmap bmp2)
        {
            using var bmp11 = ChangeResolution(bmp1, maxSize);
            using var bmp22 = ChangeResolution(bmp2, maxSize);

            if (Corral(bmp11, bmp22) > 0.75f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private unsafe static float Corral(Bitmap bmp1, Bitmap bmp2)
        {
            Bitmap bmpBig = null;
            Bitmap bmpSmall = null;
            int bmp1Size = bmp1.Width + bmp1.Height;
            int bmp2Size = bmp2.Width + bmp2.Height;
            if (bmp1Size > bmp2Size)
            {
                bmpBig = ImageTo32bpp(bmp1);
                bmpSmall = ImageTo32bpp(bmp2);
            }
            else
            {
                bmpBig = ImageTo32bpp(bmp2);
                bmpSmall = ImageTo32bpp(bmp1);
            }

            var bmpDataBig = bmpBig.LockBits(new Rectangle(0, 0, bmpBig.Width, bmpBig.Height), ImageLockMode.ReadWrite, bmpBig.PixelFormat);
            var ptrBig = bmpDataBig.Scan0;
            var rgbBitmapBig = (byte*)ptrBig;

            var bmpDataSmall = bmpSmall.LockBits(new Rectangle(0, 0, bmpSmall.Width, bmpSmall.Height), ImageLockMode.ReadWrite, bmpSmall.PixelFormat);
            var ptrSmall = bmpDataSmall.Scan0;
            var rgbBitmapSmall = (byte*)ptrSmall;

            ///////////

            var op = Vector3.Zero;
            var o1 = Vector3.Zero;
            var p1 = Vector3.Zero;
            var o2 = Vector3.Zero;
            var p2 = Vector3.Zero;

            int n = 0;
            for (var y = 0; y < bmpBig.Height && y < bmpSmall.Height; y++)
            {
                for (var x = 0; x < bmpBig.Width && x < bmpSmall.Width; x++)
                {

                    byte* addrOriginal = (byte*)(rgbBitmapBig + y * bmpDataBig.Stride + x * 4);
                    byte* addrPartical = (byte*)(rgbBitmapSmall + y * bmpDataSmall.Stride + x * 4);

                    var o = new Vector3(addrOriginal[2], addrOriginal[1], addrOriginal[0]);
                    var p = new Vector3(addrPartical[2], addrPartical[1], addrPartical[0]);

                    op += o * p;
                    o1 += o;
                    p1 += p;
                    o2 += o * o;
                    p2 += p * p;

                    n++;
                }
            }

            ///////////

            bmpBig.UnlockBits(bmpDataBig);
            bmpSmall.UnlockBits(bmpDataSmall);

            ///////////

            var up = n * op - o1 * p1;
            var dowm = (n * o2 - o1 * o1) * (n * p2 - p1 * p1);
            dowm = Vector3.SquareRoot(dowm);

            var resultVect = up / dowm;

            var result = resultVect.X + resultVect.Y + resultVect.Z;

            return result / 3.0f;
        }
    }
}
