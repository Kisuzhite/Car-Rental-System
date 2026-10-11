using System.Text;

namespace Car_Rental_System.Helpers
{
    internal static class AppLogger
    {
        private static readonly object SyncRoot = new();

        public static string LogDirectory { get; } = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            AppConstants.DefaultBusinessName,
            "Logs");

        public static string CurrentLogFilePath => Path.Combine(
            LogDirectory,
            $"app-{DateTime.Now:yyyyMMdd}.log");

        public static void LogException(string context, Exception exception)
        {
            Write("ERROR", context, exception.ToString());
        }

        public static void LogMessage(string level, string context, string message)
        {
            Write(level, context, message);
        }

        private static void Write(string level, string context, string message)
        {
            try
            {
                Directory.CreateDirectory(LogDirectory);

                var builder = new StringBuilder()
                    .Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"))
                    .Append(" [")
                    .Append(level)
                    .Append("] ")
                    .AppendLine(context)
                    .AppendLine(message)
                    .AppendLine();

                lock (SyncRoot)
                {
                    File.AppendAllText(CurrentLogFilePath, builder.ToString());
                }
            }
            catch
            {
                // Logging must never crash the application.
            }
        }
    }
}
