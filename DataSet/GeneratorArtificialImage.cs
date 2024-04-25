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

                if (settings[i].Blur)
                {
                    image = Blur(image);
                }

                if (settings[i].Noise)
                {
                    image = Noise(image);
                }

                if (settings[i].Contrast != 0)
                {
                    image = Contrast(image, settings[i].Contrast);
                }

                resultImages.Add(DataSetPhoto.ImageTo32bpp(image));
            }

            return resultImages;
        }

        private static Bitmap Rotate(Bitmap originalImage, int angle)
        {
            var rotation = new RotateBilinear(angle);
            return rotation.Apply(originalImage);
        }

        private static Bitmap Reflection(Bitmap originalImage, GeneratorArtificialImageSetting.ReflectionStruct reflection)
        {
            var mirror = new Mirror(reflection.X, reflection.Y);
            return mirror.Apply(originalImage);
        }

        private static Bitmap Blur(Bitmap originalImage)
        {
            var blur = new GaussianBlur();
            return blur.Apply(originalImage);
        }

        private static Bitmap Noise(Bitmap originalImage)
        {
            var noise = new AdditiveNoise();
            return noise.Apply(originalImage);
        }

        private static Bitmap Contrast(Bitmap originalImage, int level)
        {
            var contrast = new ContrastCorrection(level);
            return contrast.Apply(originalImage);
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

            public bool Blur { get; set; } = false;
            public bool Noise { get; set; } = false;
            public int Contrast { get; set; } = 0;


            public string GetCodeAction()
            {
                var codeAction = "_";

                if (RotateAngle != 0)
                {
                    codeAction += "-R" + RotateAngle;
                }

                if (Reflection.X)
                {
                    codeAction += "-RX";
                }

                if (Reflection.Y)
                {
                    codeAction += "-RY";
                }

                if (Blur)
                {
                    codeAction += "-B";
                }

                if (Noise)
                {
                    codeAction += "-N";
                }

                if (Contrast != 0)
                {
                    codeAction += "-C" + Contrast;
                }

                return codeAction;
            }
        }
    }
}
