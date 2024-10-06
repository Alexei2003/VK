using Newtonsoft.Json;

namespace MyCustomClasses
{
    public static class Logs
    {
        const string FILE_NAME = "log.txt";

        public static  void WriteExcemption(Exception e)
        {
            using (FileStream fs = File.OpenWrite(FILE_NAME))
            {
                // Можно записать что-то в файл, если нужно
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLine("Message");
                    writer.WriteLine(e.Message);
                    writer.WriteLine();
                    writer.WriteLine("StackTrace");
                    writer.WriteLine(e.StackTrace);
                    writer.WriteLine();
                    writer.WriteLine("##################################################################################################################################");
                    writer.WriteLine();
                }
            }
        }
    }
}
