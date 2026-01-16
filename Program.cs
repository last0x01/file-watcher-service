using System;
using System.ServiceProcess;

namespace FileMonitoringService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //static void Main()
        //{
        //    ServiceBase[] ServicesToRun;
        //    ServicesToRun = new ServiceBase[]
        //    {
        //        new FileMonitoringService()
        //    };
        //    ServiceBase.Run(ServicesToRun);
        //}


        static void Main()
        {
            if (Environment.UserInteractive)
            {
                FileMonitoringService service = new FileMonitoringService();

                Console.WriteLine("Start Debugging Mode....\n");
                service.StartInConsole();
            }
            else
            {

                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new FileMonitoringService()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }

    }
}
