# 🗂️ FileMonitoringService

[![.NET](https://img.shields.io/badge/.NET-4.7+-blue)](https://dotnet.microsoft.com/)
[![Windows Service](https://img.shields.io/badge/Windows%20Service-Ready-green)]()

Automated Windows Service for monitoring a folder, renaming files with GUIDs, and moving them to a destination folder.

---

## 📌 Overview
- Monitors a **source folder** for new files  
- Renames files using a **GUID**  
- Moves files to a **destination folder** and deletes originals  
- Logs all events to a **log folder**  
- Supports **console mode** for debugging  
- Configurable via `App.config`  

---

## 📝 Requirements
- Windows 10 / 11 or Server editions  
- .NET Framework 4.7+  
- Admin privileges to create/manage Windows Service  
- Service account with **write access** to source, destination, and log folders  

---

## ⚙ Configuration (`App.config`)
```xml
<appSettings>
  <add key="SourceFolder" value="C:\FileMonitoring\Source" />
  <add key="DestinationFolder" value="C:\FileMonitoring\Destination" />
  <add key="LogFolder" value="C:\FileMonitoring\Logs" />
</appSettings>

🚀 Installation & Commands
Create Service
sc create FileMonitoringService binPath= "C:\Path\To\FileMonitoringService.exe" start= auto

Start Service
sc start FileMonitoringService

Stop Service
sc stop FileMonitoringService

Delete Service
sc delete FileMonitoringService

📄 Logging

Sample output:

[2026-01-16 14:10:00] Service Started.
[2026-01-16 14:10:10] File detected: C:\FileMonitoring\Source\file1.txt
[2026-01-16 14:10:11] File moved: C:\FileMonitoring\Source\file1.txt -> C:\FileMonitoring\Destination\3f4d1e2a-d726.txt
[2026-01-16 14:11:15] Service Stopped.

🗂 Project Structure
FileMonitoringService/
│
├─ FileMonitoringService.cs
├─ Program.cs
├─ ProjectInstaller.cs
├─ App.config
└─ README.md

