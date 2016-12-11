using System;
using System.IO;

namespace PodHead
{
    public static class RSSConfig
    {
        private const string AndroidFolder = @"/sdcard/Android/data/PodHead.Android.PodHead.Android";

        public static string DownloadFolder
        {
            get
            {
                //var path = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "/PodHead/";
                var path = AndroidFolder + "/PodHead/Music";

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
                //var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/PodHead/";
                var path = AndroidFolder + "/PodHead/";
                if (!Directory.Exists(path))
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
