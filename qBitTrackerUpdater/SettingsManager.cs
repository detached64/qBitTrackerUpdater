using System.Text.Json;
using System.Text.Json.Serialization;

namespace qBitTrackerUpdater
{
    public sealed class SettingsManager
    {
        private static readonly string SettingsPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Settings.json";

        public static Settings Settings { get; private set; }
        private SettingsManager() { }

        static SettingsManager()
        {
            LoadSettings();
        }

        private static void LoadSettings()
        {
            if (!File.Exists(SettingsPath))
            {
                SaveDefaultSettings();
            }
            else
            {
                string json = File.ReadAllText(SettingsPath);
                try
                {
                    Settings = JsonSerializer.Deserialize(json, SourceGenerationContext.Default.Settings);
                }
                catch
                {
                    Logger.Error("Failed to parse settings file. Use default settings.");
                    Settings = new Settings();
                }
            }
        }

        private static void SaveDefaultSettings()
        {
            Logger.Info("Settings file not found. Creating default settings.");
            try
            {
                Settings = new Settings();
                File.WriteAllText(SettingsPath, JsonSerializer.Serialize(Settings, SourceGenerationContext.Default.Settings));
                Logger.Info("Default settings created.");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to save default settings: {ex.Message}");
            }
        }
    }

    public sealed class Settings
    {
        private static readonly string DefaultQBitConfigPathWindows = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\qBittorrent\\qBittorrent.ini";

        public string QBitConfigPath { get; set; } = DefaultQBitConfigPathWindows;
        public string[] TrackerUrls { get; set; } = [];
        public bool MergeTrackers { get; set; } = true;
        public bool UpdateRegularly { get; set; }
        public int UpdateInterval { get; set; } = 1; // in minutes
        public bool UseProxy { get; set; }
        public string ProxyUrl { get; set; } = string.Empty;
    }

    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(Settings))]
    public partial class SourceGenerationContext : JsonSerializerContext;
    // More information about JsonSourceGenerationOptions can be found here: https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/source-generation
}
