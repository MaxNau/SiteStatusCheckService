using System;
using System.ServiceProcess;
using Microsoft.Owin.Hosting;
using System.Timers;

namespace SiteStatusCheckService
{
    public partial class WebsiteStatusCheckService : ServiceBase
    {
        private IDisposable webApp;
        private static Status status;

        Scheduler scheduler;
        WebsiteStatusCheckClient client;

        public WebsiteStatusCheckService()
        {
            InitializeComponent();
            scheduler = new Scheduler();
            client = new WebsiteStatusCheckClient("http://localhost:8080");
        }

        // Returns Status of the Windows Service
        public static Status Status
        {
            get { return status; }
        }

        protected override void OnStart(string[] args)
        {
            // Set service Status ti Running
            status = Status.Running;

            // Write to the log file that service have been started
            ServiceLog.WriteLog("Service started");

            // Check google.com status in N-minutes interval
            scheduler.ExecuteEachNSeconds(120, EveryNMinutes_TimerTick, "api/status/checkwebsitestatus/google.com");

            // Check apple.com status in N-minutes interval
            scheduler.ExecuteEachNSeconds(300, EveryNMinutes_TimerTick, "api/status/checkwebsitestatus/www.apple.com");

            // Check microsoft.com status in N-day interval at specific time
            scheduler.ExecuteOnDayBasisAtSpecificTime(2, 22, 15, SpecifiedTime_TimeTicker, "api/status/checkwebsitestatus/microsoft.com");

            string baseUri = "http://localhost:8080";

            Console.WriteLine("Starting web Server...");

            webApp = WebApp.Start<Startup>(baseUri);

            Console.WriteLine("Web Server started and running at {0}", baseUri);  
        }

        public void OnDebug(string[] args)
        {
            OnStart(args);
           // Console.ReadLine();
        }

        private void EveryNMinutes_TimerTick(object sender, ElapsedEventArgs e, string s)
        {
            client.GetWebsiteStatus(s);
        }

        private void SpecifiedTime_TimeTicker(object sender, ElapsedEventArgs e, string s)
        {
            Console.WriteLine("Run: " + DateTime.Now);
            Scheduler.Check();
            client.GetWebsiteStatus(s);
        }

        protected override void OnStop()
        {
            // Dispose web server
            webApp.Dispose();

            // Change service Status to Stopped
            status = Status.Stopped;
        }
    }

    // Service Status
    public enum Status
    {
        Stopped,
        Running
    }
}
