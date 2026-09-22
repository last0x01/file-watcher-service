# File Monitoring Service

![.NET](https://img.shields.io/badge/.NET%20Framework-4.7%2B-blue)
![C#](https://img.shields.io/badge/language-C%23-178600?logo=csharp&logoColor=white)
![Windows Service](https://img.shields.io/badge/Windows%20Service-Ready-green)

An automated **Windows Service** that watches a **source folder** for newly created files, **renames each one with a unique GUID** (preserving its extension), and **moves it to a destination folder** — with full logging.

---

## Overview

- Monitors a source folder in real time using `FileSystemWatcher`
- Detects new files (`Created` event) as soon as they appear
- Renames files to a **GUID** while keeping the original extension (e.g. `file1.txt` → `3f4d1e2a-7bd1-4b9c-9cf4-2c6d3f5e8a01.txt`)
- Moves the renamed file to a destination folder
- Auto-creates source/destination/log folders on start
- Writes every event to `Logs.txt` (and to the Windows Event Log on failure)
- Runs as a native Windows Service, with **console mode** for debugging
- Fully configurable through `App.config`

---

## Project Structure

```
FileMonitoringService.sln
├── Program.cs                    # Service entry point (console vs. service mode)
├── FileMonitoringService.cs      # FileSystemWatcher + core move logic
├── clsUtilities.cs               # GUID generation + file move helpers
├── ProjectInstaller.cs           # Windows Service installer metadata
├── App.config                    # Source/destination/log folder settings
└── Properties/
```

---

## Requirements

- Windows 10 / 11 or Windows Server
- .NET Framework 4.7+
- Admin privileges to create/manage the Windows Service
- The service account needs **write access** to the source, destination, and log folders

---

## Configuration (`App.config`)

| Key                 | Description                                  | Example                                  |
| ------------------- | -------------------------------------------- | ---------------------------------------- |
| `SourceFolder`      | Folder to watch for new files                | `C:\FileMonitoring\Source`                |
| `DestinationFolder` | Folder where renamed files are moved         | `C:\FileMonitoring\Destination`           |
| `LogFolder`         | Where `Logs.txt` is written                  | `C:\FileMonitoring\Logs`                  |

```xml
<appSettings>
  <add key="SourceFolder" value="C:\FileMonitoring\Source" />
  <add key="DestinationFolder" value="C:\FileMonitoring\Destination" />
  <add key="LogFolder" value="C:\FileMonitoring\Logs" />
</appSettings>
```

---

## Setup

### 1. Build the project

Open `FileMonitoringService.sln` in Visual Studio and build the solution.

### 2. Install the service

```cmd
sc create FileMonitoringService binPath= "C:\Path\To\FileMonitoringService.exe" start= auto
```

### 3. Manage the service

```cmd
sc start   FileMonitoringService   // Start
sc stop    FileMonitoringService   // Stop
sc delete  FileMonitoringService   // Remove
```

### 4. Run in console mode (debugging)

Running the `.exe` directly starts it interactively — useful for testing without installer/admin setup:

```cmd
FileMonitoringService.exe
```

---

## Logging

Sample output written to `Logs.txt`:

```text
[2026-01-16 14:10:00] Service Started.
[2026-01-16 14:10:10] File detected : C:\FileMonitoring\Source\file1.txt
[2026-01-16 14:10:11] File moved : C:\FileMonitoring\Source\file1.txt -> C:\FileMonitoring\Destination\3f4d1e2a-7bd1-4b9c-9cf4-2c6d3f5e8a01.txt
[2026-01-16 14:11:15] Service Stopped.
```