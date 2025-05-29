using System.Runtime.InteropServices;

namespace qBitTrackerUpdater
{
    public sealed class PlatformManager
    {
        public static readonly string Platform;

        public static readonly string DefaultQBitConfigPath;

        private static readonly string DefaultConfigPathWindows =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "qBittorrent", "qBittorrent.ini");

        private static readonly string DefaultConfigPathLinux =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "qBittorrent", "qBittorrent.conf");

        static PlatformManager()
        {
            Platform = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "Windows"
                     : RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "Linux"
                     : RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "macOS"
                     : throw new PlatformNotSupportedException($"Unsupported platform: {RuntimeInformation.OSDescription}");
            DefaultQBitConfigPath = Platform switch
            {
                "Windows" => DefaultConfigPathWindows,
                "Linux" => DefaultConfigPathLinux,
                _ => throw new PlatformNotSupportedException($"Unsupported platform: {Platform}"),
            };
        }
    }
}
