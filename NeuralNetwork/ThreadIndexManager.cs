using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    public static class ThreadIndexManager
    {
        private static int counter = 0;
        private static Lazy<ThreadLocal<int>> lazyThreadIndex = new Lazy<ThreadLocal<int>>(() =>
            new ThreadLocal<int>(() => Interlocked.Increment(ref counter) - 1));

        public static int ThreadIndex { get => lazyThreadIndex.Value.Value; }

        public static void Cleanup()
        {
            if (lazyThreadIndex.IsValueCreated)
            {
                lazyThreadIndex.Value.Dispose();
                counter = 0;
            }
        }
    }
}
