using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;

namespace FileMonitoringService
{
    public partial class FileMonitoringService : ServiceBase
    {
        string _LogDirectory = ConfigurationManager.AppSettings["LogFolder"];
        string _LogFilePath;

        static string _SourceFolder = ConfigurationManager.AppSettings["SourceFolder"];
        static string _DestinationFolder = ConfigurationManager.AppSettings["DestinationFolder"];



        FileSystemWatcher _Watcher;

        public FileMonitoringService()
        {
            InitializeComponent();
        }



        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            string SourceFilePath;
            string DestinationFilePath;

            try
            {
                SourceFilePath = e.FullPath;



                WriteLog($"File detected : {SourceFilePath}");

                if (clsUtilities.MoveFileToDestination(SourceFilePath, _DestinationFolder, out DestinationFilePath))
                {
                    WriteLog($"File moved : {SourceFilePath} -> {DestinationFilePath}");
                }
                else
                {
                    WriteLog(($"Failed to move : {SourceFilePath}"));
                }

            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("FileMonitoringService", ex.Message, EventLogEntryType.Error);
            }
        }

        protected override void OnStart(string[] args)
        {
            CreateSourceAndDestinationFolderIfNotExists();

            _Watcher = new FileSystemWatcher(_SourceFolder)
            {
                NotifyFilter = NotifyFilters.FileName,

                Filter = "*.*",
                EnableRaisingEvents = true
            };

            _Watcher.Created += OnFileCreated;

            WriteLog("Started Service");
        }

        protected override void OnStop()
        {
            WriteLog("Stopped Service");

            if (_Watcher != null)
            {
                _Watcher.EnableRaisingEvents = false;
                _Watcher.Dispose();
            }
        }

        private void WriteLog(string Message)
        {
            CombineFilePath();

            string LogMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {Message}\n";

            try
            {
                File.AppendAllText(_LogFilePath, LogMessage);
            }
            catch (IOException iox)
            {
                EventLog.WriteEntry("FileMonitoringService", iox.Message, EventLogEntryType.Error);
            }

            if (Environment.UserInteractive)
            {
                Console.WriteLine(LogMessage);
            }

        }

        private void CombineFilePath()
        {
            if (!Directory.Exists(_LogDirectory))
                Directory.CreateDirectory(_LogDirectory);

            _LogFilePath = Path.Combine(_LogDirectory, "Logs.txt");
        }


        public void StartInConsole()
        {

            Console.WriteLine("Start-Pending...");
            OnStart(null);

            Console.ReadLine();
            Console.WriteLine("Stop-Pending...");
            OnStop();

            Console.ReadKey();


        }

        private void CreateSourceAndDestinationFolderIfNotExists()
        {
            if (!Directory.Exists(_SourceFolder))
                Directory.CreateDirectory(_SourceFolder);

            if (!Directory.Exists(_DestinationFolder))
                Directory.CreateDirectory(_DestinationFolder);
        }
    }
}
