# qBitTrackerUpdater

A simple tool to update qBitTorrent trackers from URLs (regularly).

## Installation

[Release](https://github.com/detached64/qBitTrackerUpdater/releases)

## Configuration

The application uses a configuration file named `Settings.json` located in the same directory as the executable. The file should contain the following structure:

```json
{
  "QBitConfigPath": "",
  "TrackerUrls": [],
  "MergeTrackers": true,
  "UpdateRegularly": false,
  "UpdateInterval": 1,
  "UseProxy": false,
  "ProxyUrl": ""
}
```

The json above contains the following fields:

- `QBitConfigPath`: Path to the qBittorrent configuration file (usually `qBittorrent.ini`).
- `TrackerUrls`: List of tracker URLs to be added.
- `MergeTrackers`: If true, merges the new trackers with existing ones; if false, replaces them.
- `UpdateRegularly`: If true, updates trackers at regular intervals; if false, updates only once at startup and exits.
- `UpdateInterval`: Interval in minutes for regular updates (only used if `UpdateRegularly` is true). Accepts integer values only.
- `UseProxy`: If true, uses a proxy for HTTP requests.
- `ProxyUrl`: URL of the proxy to use (only used if `UseProxy` is true).

## Notice

- If executed, the application will read the configuration file. If not found, it will create a default one. The settings created contain default values except for `QBitConfigPath`, which is completed automatically.
- By default, the application prints nothing to the console. Check the log file `qBitTrackerUpdater.log` in the same directory for output.

## Build

- dotnet sdk 9.0

- Windows
  `dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true --self-contained true`

## License

MIT License