using System;

namespace Europa.Extensions
{
    public static class IntExtensions
    {
        public static string MinutesToDuration(this int minutes)
        {
            var time = TimeSpan.FromMinutes(minutes);
            return time.ToDuration();
        }
    }
}
