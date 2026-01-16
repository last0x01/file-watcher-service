using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace FileMonitoringService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        private ServiceInstaller _ServiceInstaller;
        private ServiceProcessInstaller _ServiceProcessInstaller;
        public ProjectInstaller()
        {
            InitializeComponent();

            _ServiceProcessInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            };

            _ServiceInstaller = new ServiceInstaller
            {

                ServiceName = "FileMonitoringService",
                DisplayName = "File Monitoring Service",
                StartType = ServiceStartMode.Automatic,
                Description = "Monitors a folder, renames files using GUID, and moves them to a destination folder.",

                ServicesDependedOn = new string[] { "EventLog" }


            };

            Installers.Add(_ServiceInstaller);
            Installers.Add(_ServiceProcessInstaller);

        }
    }
}
