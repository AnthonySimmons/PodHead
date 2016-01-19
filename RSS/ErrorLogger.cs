using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSS
{
    public delegate void ErrorFoundEventHandler(string message);
    
    public static class ErrorLogger
    {
        public static event ErrorFoundEventHandler ErrorFound;

        public static string ErrorLogPath = Path.Combine(RSSConfig.AppDataFolder, "ErrorLog.txt");

        public static void Log(Exception ex)
        {
            string msg = string.Empty;
            try
            {
                if (ex != null)
                {
                    msg = ex.Message;
                    if (ex.InnerException != null)
                    {
                        msg += "\n " + ex.InnerException.Message;
                    }
                    msg += "\n " + ex.StackTrace;
                    Log(msg);
                }
            }
            catch { }
        }

        public static void Log(string message)
        {
            using (var writer = File.Open(ErrorLogPath, FileMode.Append | FileMode.Create))
            {
                var dateBytes = Encoding.UTF8.GetBytes(DateTime.Now.ToLongDateString());
                var messageBytes = Encoding.UTF8.GetBytes(message);
                writer.Write(dateBytes, 0, dateBytes.Length);
                writer.Write(messageBytes, 0, messageBytes.Length);
                OnErrorFound(message);
            }
        }

        private static void OnErrorFound(string message)
        {
            var copy = ErrorFound;
            if(copy != null)
            {
                copy.Invoke(message);
            }
        }
    }
}
