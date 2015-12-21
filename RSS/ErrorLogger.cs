using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSS
{
    public static class ErrorLogger
    {
        public static string ErrorLogPath => Path.Combine(RSSConfig.AppDataFolder, "ErrorLog.txt");

        public static void Log(Exception ex)
        {
            Log($"{ex?.Message}: \n {ex?.InnerException?.Message} \n {ex?.StackTrace}");
        }

        public static void Log(string message)
        {
            using (var writer = File.Open(ErrorLogPath, FileMode.Append | FileMode.Create))
            {
                var bytes = Encoding.UTF8.GetBytes(message);
                writer.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
