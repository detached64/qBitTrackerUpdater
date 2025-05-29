using System.Reflection;
using System.Text;

namespace qBitTrackerUpdater
{
    public static class Logger
    {
        private static readonly string LogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.log");

        private static readonly Lock _lockObject = new();

        private static void AppendLine(string message)
        {
            try
            {
                string trimed = message.Trim();
#if DEBUG
                Console.WriteLine(trimed);
#endif
                lock (_lockObject)
                {
                    using var writer = new StreamWriter(LogPath, true, Encoding.UTF8);
                    writer.WriteLine(trimed);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write to log: {ex.Message}");
            }
        }

        public static void Info(string message)
        {
            AppendLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}][INFO] {message}");
        }

        public static void Error(string message)
        {
            AppendLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}][ERROR] {message}");
        }
    }
}
