namespace Other
{
    public static class Logs
    {
        const string FileName = "log.txt";

        public static void WriteException(Exception e, string caption = "")
        {
            lock (FileName)
            {
                using var writer = File.AppendText(FileName);
                writer.WriteLine(caption);
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
}
