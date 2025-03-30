﻿using System.Numerics;

using DataSet;

using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace NeuralNetwork
{
    public static class NeuralNetworkWorker
    {
        private const int MAX_SESSIONS_NVIDIA = 4;
        private const int MAX_GPU_INTEL = 3;
        private const int MAX_CPU = 0;
        private const int MAX_SESSIONS = MAX_SESSIONS_NVIDIA + MAX_GPU_INTEL + MAX_CPU;

        private static Session[] _sessionArr = new Session[MAX_SESSIONS];

        private static readonly string _inputName;

        // Статическая модель и метки классов
        public static string[] Labels { get; private set; }


        private class Session
        {
            public bool IsBusy { get; set; } = false;
            public bool IsFirst { get; set; } = true;

            public InferenceSession Inference { get; set; }

            public Session(InferenceSession inference)
            {
                Inference = inference;
            }
        }

        // Статический конструктор для инициализации
        static NeuralNetworkWorker()
        {
            // Загружаем модель 
            var i = 0;
            try
            {
                // GPU Nvidia
                // Пытаемся использовать DirectML (работает через DirectX 12)
                var options = new SessionOptions();
                options.AppendExecutionProvider_DML(0);
                for (; i < MAX_SESSIONS_NVIDIA; i++)
                {
                    _sessionArr[i] = new Session(new InferenceSession("model.onnx", options));
                }

                // GPU Intel
                // Пытаемся использовать DirectML (работает через DirectX 12)
                options = new();
                options.AppendExecutionProvider_DML(1);
                for (; i < MAX_SESSIONS_NVIDIA + MAX_GPU_INTEL; i++)
                {
                    _sessionArr[i] = new Session(new InferenceSession("model.onnx", options));
                }

                //CPU Intel
                for (; i < MAX_SESSIONS_NVIDIA + MAX_GPU_INTEL + MAX_CPU; i++)
                {
                    _sessionArr[i] = new Session(new InferenceSession("model.onnx"));
                }
            }
            catch
            {
                // Fallback на CPU
                for (; i < MAX_SESSIONS; i++)
                {
                    _sessionArr[i] = new Session(new InferenceSession("model.onnx"));
                }
            }

            _inputName = _sessionArr[0].Inference.InputMetadata.Keys.First();

            // Загружаем метки классов
            string labelFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "labels.txt");
            if (File.Exists(labelFilePath))
            {
                Labels = File.ReadAllLines(labelFilePath);
            }
            else
            {
                Labels = ["#unknown"]; // На случай, если файл отсутствует
            }
        }

        private struct Label
    {
        public string Name;
        public float Value;

        public Label(string name, float value)
        {
            Name = name;
            Value = value;
        }
        }

        public static string NeuralNetworkResult(Image<Rgb24> imageOriginal)
        {
            var arr = NeuralNetworkResultKTopCount(imageOriginal, 1);

            return arr[0];
        }

        public static string[] NeuralNetworkResultKTopPercent(Image<Rgb24> imageOriginal, double percent = 0.10)
        {
            var kTop = int.Max((int)(Labels.Length * percent), 1);

            var arr = NeuralNetworkResultKTopCount(imageOriginal, kTop);
            return arr;
        }

        public static string[] NeuralNetworkResultKTopCount(Image<Rgb24> imageOriginal, int kTop = 15)
        {
            using var image = DataSetImage.ChangeResolution224x224(imageOriginal);

            var inputTensor = ImageToTensor(image);

            var input = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor(_inputName, inputTensor)
            };

            while (true)
            {
                var session = _sessionArr.FirstOrDefault(s => s.IsBusy == false);

                if (session == null)
                {
                    Thread.Sleep(100);
                    continue;
                }

                session.IsBusy = true;
                float[] outputArr;
                lock (session)
                {
                    if(session.IsFirst == true)
                    {
                        var rand = new Random();
                        Thread.Sleep(rand.Next(MAX_SESSIONS * 100));

                        session.IsFirst = false;
                    }
                    using var results = session.Inference.Run(input);
                    outputArr = [.. results[0].AsEnumerable<float>()];
                }
                session.IsBusy = false;

                var labels = new Label[outputArr.Length];

                for (var i = 0; i < outputArr.Length; i++)
                {
                    labels[i] = new Label(Labels[i], outputArr[i]);
                }

                var resulTagsArr = labels.OrderByDescending(l => l.Value).Take(kTop).Select(l => l.Name);

                if (resulTagsArr.Contains("#nsfw"))
                {
                    resulTagsArr = ["#nsfw"];
                }

                return [.. resulTagsArr];
            }
        }




        public static DenseTensor<float> ImageToTensor(Image<Rgb24> bitmap)
        {
            const int batchSize = 1;
            const int height = 224;
            const int width = 224;
            const int channels = 3;

            var tensor = new DenseTensor<float>([batchSize, height, width, channels]);

            var constVect = new Vector3(255f);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var pixel = bitmap[x, y];
                    var vect = new Vector3(pixel.B, pixel.G, pixel.R);
                    vect = vect / constVect;
                    tensor[0, y, x, 0] = vect[0];
                    tensor[0, y, x, 1] = vect[1];
                    tensor[0, y, x, 2] = vect[2];
                }
            }

            return tensor;
        }
    }
}