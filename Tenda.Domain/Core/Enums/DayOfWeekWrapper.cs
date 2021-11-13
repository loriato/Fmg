using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tenda.Domain.Core.Enums
{
    public static class DayOfWeekWrapper
    {
        // InnerData
        private static IDictionary<DayOfWeek, int> _domain = new Dictionary<DayOfWeek, int>();

        // Concurrency control
        private static object _lock = new object();

        // Decompose Strategy
        private const int Sunday = 1;
        private const int Monday = 2;
        private const int Tuesday = 4;
        private const int Wednesday = 8;
        private const int Thursday = 16;
        private const int Friday = 32;
        private const int Saturday = 64;

        // Inicializer
        private static IDictionary<DayOfWeek, int> Values()
        {
            if (_domain.IsEmpty())
            {
                lock (_lock)
                {
                    if (_domain.IsEmpty())
                    {
                        _domain.Add(DayOfWeek.Sunday, Sunday);
                        _domain.Add(DayOfWeek.Monday, Monday);
                        _domain.Add(DayOfWeek.Tuesday, Tuesday);
                        _domain.Add(DayOfWeek.Wednesday, Wednesday);
                        _domain.Add(DayOfWeek.Thursday, Thursday);
                        _domain.Add(DayOfWeek.Friday, Friday);
                        _domain.Add(DayOfWeek.Saturday, Saturday);
                    }
                }
            }
            return _domain;
        }

        public static int FromDomain(DayOfWeek key)
        {
            return Values()
                .Where(reg => reg.Key == key)
                .Select(reg => reg.Value)
                .SingleOrDefault();
        }

        public static List<DayOfWeek> GetDays(int sum)
        {
            List<DayOfWeek> days = new List<DayOfWeek>();
            if (sum <= 0)
            {
                return new List<DayOfWeek>();
            }
            if (sum >= Saturday)
            {
                days.Add(DayOfWeek.Saturday);
                sum -= Saturday;
            }
            if (sum >= Friday)
            {
                days.Add(DayOfWeek.Friday);
                sum -= Friday;
            }
            if (sum >= Thursday)
            {
                days.Add(DayOfWeek.Thursday);
                sum -= Thursday;
            }
            if (sum >= Wednesday)
            {
                days.Add(DayOfWeek.Wednesday);
                sum -= Wednesday;
            }
            if (sum >= Tuesday)
            {
                days.Add(DayOfWeek.Tuesday);
                sum -= Tuesday;
            }
            if (sum >= Monday)
            {
                days.Add(DayOfWeek.Monday);
                sum -= Monday;
            }
            if (sum >= Sunday)
            {
                days.Add(DayOfWeek.Sunday);
            }
            return days;
        }
    }
}
