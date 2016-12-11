using PodHead;
using System;
using System.IO;

namespace PodHeadForms
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
                lock(_lock)
                {
                    if(_instance == null)
                    {
                        _instance = new Config();
                    }
                }

                return _instance;
            }
        }

        public string DownloadFolder
        {
            get
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "PodHead");
               
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
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PodHead");
               
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
