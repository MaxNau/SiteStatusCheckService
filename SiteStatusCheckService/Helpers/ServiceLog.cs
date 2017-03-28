using System;
using System.IO;

namespace SiteStatusCheckService
{
    public static class ServiceLog
    {
        private static object locker = new object();

        /// <summary>
        /// Writes service log
        /// </summary>
        /// <param name="message"> service log message </param>
        public static void WriteLog(string message)
        {
            lock (locker)
            {
                using (StreamWriter sw = new StreamWriter(@"C:\Logfile.txt", true))
                {
                    sw.WriteLine(DateTime.Now.ToString() + ":" + message);
                    sw.Flush();
                }
            }
        }
    }
}
