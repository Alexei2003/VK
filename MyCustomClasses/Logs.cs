﻿namespace MyCustomClasses
{
    public static class Logs
    {
        const string FILE_NAME = "log.txt";

        public static void WriteExcemption(Exception e)
        {
            using (StreamWriter writer = File.AppendText(FILE_NAME))
            {
                writer.WriteLine("Date: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                writer.WriteLine();
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
