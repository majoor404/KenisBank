using System;
using System.IO;

namespace KenisBank
{
    internal static class Logger
    {
        private static readonly object _lock = new object();
        private static readonly string LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "app.log");

        public static void Log(string message)
        {
            try
            {
                lock (_lock)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(LogFilePath));
                    File.AppendAllText(LogFilePath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}");
                }
            }
            catch
            {
                // best effort logging, swallow errors to avoid cascading failures
            }
        }

        public static void Log(Exception ex, string context = null)
        {
            try
            {
                string msg = context == null ? ex.ToString() : $"{context} - {ex}";
                Log(msg);
            }
            catch
            {
            }
        }
    }
}
