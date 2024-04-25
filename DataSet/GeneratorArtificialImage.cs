using AForge.Imaging.Filters;
using DataSet;
using System.Drawing;

namespace AddDataInDataSet
{
    public static class GeneratorArtificialImage
    {
        public static List<Bitmap> Generate(Bitmap originalImage, GeneratorArtificialImageSetting[] settings, int maxImages = 100000000)
        {
            var resultImages = new List<Bitmap>();

            originalImage = DataSetPhoto.ImageTo24bpp(originalImage);

            for (var i = 0; i < settings.Length && i < maxImages; i++)
            {
                Bitmap image = originalImage;
                if (settings[i].RotateAngle != 0)
                {
                    image = Rotate(image, settings[i].RotateAngle);
                }

                if (settings[i].Reflection.X != false || settings[i].Reflection.Y != false)
                {
                    image = Reflection(image, settings[i].Reflection);
                }

                resultImages.Add(DataSetPhoto.ImageTo32bpp(image));
            }

            return resultImages;
        }

        private static Bitmap Rotate(Bitmap originalImage, int angle)
        {
            var rotationFilter = new RotateBilinear(angle);
            return rotationFilter.Apply(originalImage);
        }

        private static Bitmap Reflection(Bitmap originalImage, GeneratorArtificialImageSetting.ReflectionStruct reflection)
        {
            var mirror = new Mirror(reflection.X, reflection.Y);
            return mirror.Apply(originalImage);
        }



        public class GeneratorArtificialImageSetting()
        {
            public int RotateAngle { get; set; } = 0;

            public struct ReflectionStruct()
            {
                public bool X { get; set; } = false;

                public bool Y { get; set; } = false;
            }

            public ReflectionStruct Reflection { get; set; } = new ReflectionStruct();
        }
    }
}
