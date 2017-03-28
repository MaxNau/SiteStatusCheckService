using System;
using System.Timers;

namespace SiteStatusCheckService
{
    public class Scheduler
    {
        private static Timer timer;
        private TimeSpan timeSchedule;

        /// <summary>
        /// Sets the timer to perform specific action repeatable on specified interval
        /// </summary>
        /// <param name="seconds"> Interval in seconds to perform specific action repeatable </param>
        /// <param name="onTimerTick"> Method named that will be called on each timer tick </param>
        /// <param name="apiUri"> web api service uri </param>
        public void ExecuteEachNSeconds(int seconds, Action<object, ElapsedEventArgs, string> onTimerTick, string apiUri)
        {
            timer = new Timer();
            timer.Interval = seconds * 1000;
            timer.Elapsed += new ElapsedEventHandler((sender, e) => onTimerTick(sender, e, apiUri));
            timer.Enabled = true;
        }

        /// <summary>
        /// Sets the timer to perform specific action on day basis at specific time
        /// </summary>
        /// <param name="days"> Number of dayes to wait before performing specified action </param>
        /// <param name="hours"> Hour at wich specified action needs to be performed </param>
        /// <param name="minutes"> Minute at which specified action need to be done </param>
        /// <param name="onTimerTick"> ethod named that will be called on each timer tick </param>
        /// <param name="apiUri"> web api service uri </param>
        public void ExecuteOnDayBasisAtSpecificTime(int days, int hours, int minutes, Action<object, ElapsedEventArgs, string> onTimerTick, string apiUri)
        {
            timer = new Timer();
            timeSchedule = new TimeSpan(days, hours, minutes, 0);
            int NextRunTime = ToMilliseconds(hours, minutes); 
            int CurentTime = ToMilliseconds(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) + DateTime.Now.Millisecond;
            int timeDifference = NextRunTime - CurentTime;
            if (timeDifference > 0)
                timer.Interval = timeDifference;
            else
                timer.Interval = (24 * 60000 * 60 - CurentTime) + NextRunTime;
            timer.Elapsed += new ElapsedEventHandler((sender, e) => onTimerTick(sender, e, apiUri));
            timer.Enabled = true;
            timer.Start();
        }

        /// <summary>
        /// Converts hours, minutes, seconds to milliseconds
        /// </summary>
        /// <param name="hours"> Numebr of hours to convert </param>
        /// <param name="minutes"> Numebr of minutes to convert </param>
        /// <param name="seconds"> Numebr of seconds to convert </param>
        /// <returns> returns total milliseconds </returns>
        private int ToMilliseconds(int hours, int minutes = 0, int seconds = 0)
        {
            int resut = hours * 60000 * 60 + minutes * 60000 + seconds * 1000;
            return resut;
        }

        /// <summary>
        /// Sets the time interval when specified action needs to be called
        /// </summary>
        /// <returns> total milliseconds to day when action should be performed </returns>
        private static double CheckSchedule()
        {
            DateTime dt = DateTime.Now.AddDays(2);

            Console.WriteLine("Next run at: " + dt.ToString());
            return dt.Subtract(DateTime.Now).TotalMilliseconds;
        }

        /// <summary>
        /// Recalculates time interval
        /// </summary>
        public static void Check()
        {
            timer.Stop();
            timer.Interval = (int)CheckSchedule();
            timer.Start();
        }
    }
}
