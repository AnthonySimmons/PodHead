using System;
using System.IO;

namespace PodHead
{
    public static class RSSConfig
    {
        private static string AndroidFolder = "/storage/extSdCard/AppData/PodHead";
        public static string DownloadFolder
        {
            get
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "/PodHead/";
                
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
                var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/PodHead/";
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
