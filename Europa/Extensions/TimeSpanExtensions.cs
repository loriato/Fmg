using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Europa.Extensions
{
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Retorna da data no formato HH:mm
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToTime(this TimeSpan time)
        {
            return string.Format("{0:hh\\:mm}", time);
        }

        /// <summary>
        /// Retorna da data no formato HH:mm:ss
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToTimeWithSeconds(this TimeSpan time)
        {
            return string.Format("{0:hh\\:mm\\:ss}", time);
        }

        public static string ToDuration(this TimeSpan time)
        {
            return string.Format("{0:000}:{1:00}", (int)time.TotalHours, time.Minutes);
        }
    }
}
