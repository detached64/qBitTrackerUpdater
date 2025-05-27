using PeanutButter.INI;
using System.Net;

namespace qBitTrackerUpdater
{
    public static class Program
    {
        private const string SectionBitTorrent = "BitTorrent";
        private const string SectionAdditionalTrackers = "Session\\AdditionalTrackers";

        private static HttpClient Client;

        private static void Main()
        {
            HandleUpdate();
        }

        private static void HandleUpdate()
        {
            if (SettingsManager.Settings.UpdateRegularly)
            {
                Timer timer = new(_ =>
                {
                    Process();
                    Logger.Info("Waiting for next update...");
                }, null, TimeSpan.FromSeconds(1), TimeSpan.FromMinutes(SettingsManager.Settings.UpdateInterval));
                Logger.Info($"Scheduled updates every {SettingsManager.Settings.UpdateInterval} minutes.");
                var exitEvent = new ManualResetEvent(false);
                exitEvent.WaitOne();
            }
            else
            {
                Process();
            }
        }

        private static void Process()
        {
            Logger.Info("Starting tracker update process...");
            try
            {
                ConfigureHttpClient();
                INIFile config = LoadFile();
                EnsureContainsSection(config);
                HashSet<string> existingTrackers = GetExistingTrackers(config);
                HashSet<string> remoteTrackers = FetchRemoteTrackers();
                string result = MergeTrackers(existingTrackers, remoteTrackers);
                SaveFile(config, result);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                Logger.Error(ex.ToString());
            }
            Logger.Info("Tracker update process completed.");
        }

        private static void ConfigureHttpClient()
        {
            if (SettingsManager.Settings.UseProxy)
            {
                HttpClientHandler handler = new()
                {
                    UseProxy = SettingsManager.Settings.UseProxy,
                    Proxy = new WebProxy(SettingsManager.Settings.ProxyUrl)
                };
                Client = new HttpClient(handler);
                Logger.Info($"HttpClient configured with proxy: {SettingsManager.Settings.ProxyUrl}");
            }
            else
            {
                Client = new HttpClient();
                Logger.Info("HttpClient configured without proxy.");
            }
        }

        private static INIFile LoadFile()
        {
            if (File.Exists(Path.GetFullPath(SettingsManager.Settings.QBitConfigPath)))
            {
                return new(SettingsManager.Settings.QBitConfigPath);
            }
            else
            {
                throw new FileNotFoundException($"qBittorrent config file not found at {SettingsManager.Settings.QBitConfigPath}. Please check your settings.");
            }
        }

        private static void EnsureContainsSection(INIFile file)
        {
            if (file.Sections.Contains(SectionBitTorrent) && file[SectionBitTorrent].ContainsKey(SectionAdditionalTrackers))
            {
                return;
            }
            throw new InvalidDataException("The qBittorrent config file does not contain the required sections or keys. Please ensure it is a valid qBittorrent configuration file.");
        }

        private static HashSet<string> GetExistingTrackers(INIFile file)
        {
            string trackers = file[SectionBitTorrent][SectionAdditionalTrackers];
            string[] existing = Utils.Split(trackers, false);
            Logger.Info($"Found {existing.Length} existing trackers.");
            return [.. existing];
        }

        private static HashSet<string> FetchRemoteTrackers()
        {
            if (SettingsManager.Settings.TrackerUrls == null || SettingsManager.Settings.TrackerUrls.Length == 0)
            {
                throw new InvalidOperationException("No tracker URLs configured. Please update your settings.");
            }

            HashSet<string> trackers = [];
            long count = 0;
            foreach (string url in SettingsManager.Settings.TrackerUrls)
            {
                try
                {
                    string content = Client.GetStringAsync(url).GetAwaiter().GetResult();
                    if (!string.IsNullOrWhiteSpace(content))
                    {
                        string[] lines = Utils.Split(content, true);
                        foreach (string line in lines)
                        {
                            trackers.Add(line.Trim());
                        }
                        count += lines.Length;
                        Logger.Info($"Fetched {lines.Length} trackers from {url}");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"Failed to fetch trackers from {url}: {ex.Message}");
                }
            }
            Logger.Info($"Total trackers fetched from all sources: {count}");
            return trackers;
        }

        private static string MergeTrackers(HashSet<string> existingTrackers, HashSet<string> remoteTrackers)
        {
            Logger.Info($"Total trackers after deduplication: {remoteTrackers.Count}");
            if (SettingsManager.Settings.MergeTrackers)
            {
                remoteTrackers.UnionWith(existingTrackers);
                Logger.Info($"Total trackers after merging: {remoteTrackers.Count}");
            }
            return Utils.Join([.. remoteTrackers], false);
        }

        private static void SaveFile(INIFile file, string content)
        {
            file[SectionBitTorrent][SectionAdditionalTrackers] = content;
            file.Persist();
        }
    }
}
