using System;
using System.ServiceProcess;

namespace SiteStatusCheckService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            // Check if we are in debug mode
            // If its true then run Windows Service in Debug mode
            // Else install Windows Service on the machine and run it
            if (Environment.UserInteractive)
            {
                WebsiteStatusCheckService websiteStatusCheckService = new WebsiteStatusCheckService();

                // Start the service
                websiteStatusCheckService.OnDebug(args);
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new WebsiteStatusCheckService()
                };
                ServiceBase.Run(ServicesToRun);
            }
            Console.ReadLine();
        }
    }
}