using System.IO;
using System;

namespace PodHead.Android
{
    public class Config : IConfig
    {
        private static Config _instance;

        private static readonly object _lock = new object();

        private Config()
        {

        }

        public static IConfig Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Config();
                    }
                }

                return _instance;
            }
        }

#if DEBUG
        private static readonly string AndroidFolder = @"/sdcard/Android/data/PodHead.Android.PodHead.Android";
#else
        private static readonly string AndroidFolder = @"/sdcard/Android/data/PodHead.Android.PodHead.Android";//Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
#endif

        public string DownloadFolder
        {
            get
            {
                var path = Path.Combine(AndroidFolder, "PodHead", "Downloads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }

        public string AppDataFolder
        {
            get
            {
                var path = Path.Combine(AndroidFolder, "PodHead");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }

        public string AppDataImageFolder
        {
            get
            {
                var path = Path.Combine(AppDataFolder, "Images");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }


        public string ConfigFileName
        {
            get
            {
                return Path.Combine(AppDataFolder, "Config.xml");
            }
        }

    }
}
