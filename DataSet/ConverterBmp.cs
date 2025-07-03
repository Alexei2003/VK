using System.Drawing;
using System.Drawing.Imaging;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.PixelFormats;

namespace DataSet
{
    public static class ConverterBmp
    {
        public static Bitmap ConvertToBitmap(Image<Rgb24> image)
        {
            using var ms = new MemoryStream();
            image.Save(ms, BmpFormat.Instance);
            ms.Position = 0; // Сброс позиции после записи

            return (Bitmap)Bitmap.FromStream(ms);
        }

        public static Image<Rgb24> ConvertToImageSharp(Bitmap image)
        {
            using var imageNew = image.Clone(new System.Drawing.Rectangle { X = 0, Y = 0, Width = image.Width, Height = image.Height }, PixelFormat.Format24bppRgb);

            using var ms = new MemoryStream();
            imageNew.Save(ms, ImageFormat.Bmp);

            ms.Position = 0; // Сброс позиции после записи

            return SixLabors.ImageSharp.Image.Load<Rgb24>(ms);
        }
    }
}
