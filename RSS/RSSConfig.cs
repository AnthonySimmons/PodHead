using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSS
{
    public static class RSSConfig
    {
        public static string DownloadFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "/RSS/";

        public static string AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/RSS/";

        public static string ConfigFileName = AppDataFolder + "Config.xml";
    }
}
