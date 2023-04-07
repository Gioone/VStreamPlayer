using System;

namespace VStreamPlayer.Helper
{
    public static class Convert
    {
        /// <summary>
        /// Convert a string clock like "10:25:30" or "10:25:30.107" to milliseconds.
        /// </summary>
        /// <param name="clock">Specified clock</param>
        /// <returns>Milliseconds</returns>
        public static long ClockToMilliseconds(string clock)
        {
            string[] arrClock = clock.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            string[] arrTime = arrClock[0].Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            int.TryParse(arrTime[0], out int hours);
            int.TryParse(arrTime[1], out int minutes);
            int.TryParse(arrTime[2], out int seconds);
            long totals;
            totals = hours * 60 * 60 * 1000;
            totals += minutes * 60 * 1000;
            totals += seconds * 1000;
            // It is like "10:25:30.107"
            if (arrClock.Length == 2)
            {
                int.TryParse(arrClock[1], out int milliseconds);

                totals += milliseconds;
                return totals;
            }
            // It is like "10:25:30"
            else
            {
                return totals;
            }
        }

        /// <summary>
        /// Convert a millisecond to a clock <see cref="string" /> like "14:53:55"
        /// </summary>
        /// <param name="ticks">A <see cref="TimeSpan" />'s milliseconds parameter</param>
        /// <returns>Clock string</returns>
        public static string MillisecondsToClock(long ticks)
        {
            long totals;
            try
            {
                totals = System.Convert.ToInt64(ticks);
            }
            catch (OverflowException)
            {
                totals = 0;
            }

            long result = totals;

            int hours = (int)(result / 1000 / 60 / 60);

            result = result - hours * 60 * 60 * 1000;

            int minutes = (int)(result / 1000 / 60);

            result = result - minutes * 60 * 1000;

            int seconds = (int)(result / 1000);

            return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
        }

        /// <summary>
        /// Return a clock with milliseconds (Like 10:45:24.137). 
        /// See also <seealso cref="MillisecondsToClock(long)" />
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns>A clock <see cref="string" />.</returns>
        public static string MillisecondsToMilliClock(long ticks)
        {
            long totals;
            try
            {
                totals = System.Convert.ToInt64(ticks);
            }
            catch (OverflowException)
            {
                totals = 0;
            }

            long result = totals;

            int hours = (int)(result / 1000 / 60 / 60);

            result = result - hours * 60 * 60 * 1000;

            int minutes = (int)(result / 1000 / 60);

            result = result - minutes * 60 * 1000;

            int seconds = (int)(result / 1000);

            result = result - seconds * 1000;

            int milliseconds = (int)result;

            return $"{hours:D2}:{minutes:D2}:{seconds:D2}.{milliseconds:D3}";
        }
    }
}
