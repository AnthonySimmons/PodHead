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
                var dateBytes = Encoding.UTF8.GetBytes(DateTime.Now.ToLongDateString());
                var messageBytes = Encoding.UTF8.GetBytes(message);
                writer.Write(dateBytes, 0, dateBytes.Length);
                writer.Write(messageBytes, 0, messageBytes.Length);
            }
        }
    }
}
