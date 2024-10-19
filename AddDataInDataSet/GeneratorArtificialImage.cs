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

                if (settings[i].GaussianBlur)
                {
                    image = GaussianBlur(image);
                }

                if (settings[i].AdditiveNoise)
                {
                    image = AdditiveNoise(image);
                }

                if (settings[i].ContrastCorrection != 0)
                {
                    image = ContrastCorrection(image, settings[i].ContrastCorrection);
                }

                if (settings[i].Resize != 0)
                {
                    image = Resize(image, settings[i].Resize);
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

        private static Bitmap GaussianBlur(Bitmap originalImage)
        {
            var blur = new GaussianBlur();
            return blur.Apply(originalImage);
        }

        private static Bitmap AdditiveNoise(Bitmap originalImage)
        {
            new AdditiveNoise();
            var noise = new AdditiveNoise();
            return noise.Apply(originalImage);
        }

        private static Bitmap ContrastCorrection(Bitmap originalImage, int level)
        {
            var contrast = new ContrastCorrection(level);
            return contrast.Apply(originalImage);
        }

        private static Bitmap Resize(Bitmap originalImage, int maxSize)
        {
            return DataSetPhoto.ChangeResolution(originalImage, maxSize);
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

            public bool GaussianBlur { get; set; } = false;
            public bool AdditiveNoise { get; set; } = false;
            public int ContrastCorrection { get; set; } = 0;

            public int Resize { get; set; } = 0;

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

                if (GaussianBlur)
                {
                    codeAction += "-GB";
                }

                if (AdditiveNoise)
                {
                    codeAction += "-AN";
                }

                if (ContrastCorrection != 0)
                {
                    codeAction += "-CС" + ContrastCorrection;
                }


                if (Resize != 0)
                {
                    codeAction += "-RS" + Resize;
                }

                return codeAction;
            }
        }
    }
}
