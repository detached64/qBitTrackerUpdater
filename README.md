# qBitTrackerUpdater

A simple tool to update qBitTorrent trackers from URLs (regularly).

## Features

- Light-weight & framework-independent
- Updates trackers from a list of URLs optionally at regular intervals
- Updates trackers from file `Extra.txt`
- Auto deduplication and merging
- Optional proxy support

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
- The application does not require qBittorrent to be running or not.
- You can create a file named `Extra.txt` in the same directory as the executable. This file can contain additional tracker URLs, one per line. The application will read this file and add the trackers to the list.
- There is a sample configuration file [here](./docs/Settings.json) which containing several avaliable URLs. It is welcomed to contribute more URLs to the list.

## Build

- dotnet sdk 9.0

- Windows

  `dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true --self-contained true`

## Acknowledgements

- [PeanutButter.INI](https://github.com/fluffynuts/PeanutButter) for parsing ini files.

## License

MIT License