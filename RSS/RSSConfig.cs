using System;
using System.IO;

namespace RSS
{
    public static class RSSConfig
    {
        private static string AndroidFolder = "/storage/extSdCard/AppData/RSS";
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
                //var path = AndroidFolder;
                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }        
        }
            
        public static string AppDataImageFolder
        {
            get
            {
                var path = Path.Combine(AppDataFolder, "Images");
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
