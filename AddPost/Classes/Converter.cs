using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.PixelFormats;
using System.Drawing.Imaging;

namespace AddPost.Classes
{
    internal static class Converter
    {
        public static Bitmap ConvertToBitmap(Image<Rgb24> image)
        {
            using var ms = new MemoryStream();
            image.Save(ms, BmpFormat.Instance);
            ms.Position = 0; // Сброс позиции после записи

            return (Bitmap)System.Drawing.Bitmap.FromStream(ms);
        }

        public static Image<Rgb24> ConvertToImageSharp(Bitmap image)
        {
            image = image.Clone(new System.Drawing.Rectangle { X = 0, Y = 0, Width = image.Width, Height = image.Height }, PixelFormat.Format24bppRgb);

            using var ms = new MemoryStream();
            image.Save(ms, ImageFormat.Bmp);

            ms.Position = 0; // Сброс позиции после записи

            return SixLabors.ImageSharp.Image.Load<Rgb24>(ms);
        }
    }
}
