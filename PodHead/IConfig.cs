

namespace PodHead
{
    public interface IConfig
    {
        string DownloadFolder { get; }

        string AppDataFolder { get; }

        string AppDataImageFolder { get; }

        string ConfigFileName { get; }
    }
}
