using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.ML.OnnxRuntime;

namespace NeuralNetwork
{
    public sealed class Session
    {
        public bool IsFirst { get; set; } = true;

        public int GPUID { get; private set; } = -1;
        private InferenceSession _inference;

        public Session(int gpuid)
        {
            GPUID = gpuid;
            _inference = CreateInferenceSession();
        }

        public float[] Run(IReadOnlyCollection<NamedOnnxValue> input)
        {
            using var results = _inference.Run(input);
            return [.. results[0].AsEnumerable<float>()];
        }

        public void Reset()
        {
            IsFirst = true;
            _inference.Dispose();
            _inference = CreateInferenceSession();
        }

        public string GetInputName()
        {
            return _inference.InputMetadata.Keys.First();
        }

        private InferenceSession CreateInferenceSession()
        {
            var options = new SessionOptions();
#if WINDOWS
            if (GPUID != -1)
            {
                // Пытаемся использовать DirectML (работает через DirectX 12)
                try
                {
                    options.AppendExecutionProvider_DML(GPUID);
                }
                catch
                {
                    GPUID = -1;
                }
            }
#endif
            lock (NeuralNetworkWorker.Labels)
            {
                while (true)
                {
                    try
                    {
                        return new InferenceSession( "E:\\WPS\\CommonData\\Model\\model.onnx", options);
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
        }

    }
}
