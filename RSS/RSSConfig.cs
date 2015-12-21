using System;
using System.IO;

namespace RSS
{
    public static class RSSConfig
    {
        public static string DownloadFolder
        {
            get
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "/RSS/";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }

        public static string AppDataFolder
        {
            get
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/RSS/";
                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }        
        }
            


        public static string ConfigFileName = AppDataFolder + "Config.xml";
    }
}
