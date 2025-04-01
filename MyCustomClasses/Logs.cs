namespace MyCustomClasses
{
    public static class Logs
    {
        const string FILE_NAME = "log.txt";

        public static void WriteException(Exception e)
        {
            using var writer = File.AppendText(FILE_NAME);
            writer.WriteLine("Date: " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
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
