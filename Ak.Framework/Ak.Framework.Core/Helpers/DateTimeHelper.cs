using System;
using System.Collections.Generic;
using System.Linq;
using Ak.Framework.Core.Extensions;
using Ak.Framework.Core.Models;

namespace Ak.Framework.Core.Helpers
{
    /// <summary>
    /// Класс для работы с датами
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// Разделение диапазона дат на промежутки
        /// </summary>
        /// <param name="start">Начало диапазона</param>
        /// <param name="end">Конец диапазона</param>
        /// <param name="dayChunkSize">Длина промежутка</param>
        /// <returns></returns>
        public static IEnumerable<Tuple<DateTime, DateTime>> SplitDateRange(DateTime start, DateTime end, int dayChunkSize)
        {
            DateTime startOfThisPeriod = start;
            while (startOfThisPeriod < end)
            {
                DateTime endOfThisPeriod = startOfThisPeriod.AddDays(dayChunkSize);
                endOfThisPeriod = endOfThisPeriod < end ? endOfThisPeriod : end;
                yield return Tuple.Create(startOfThisPeriod, endOfThisPeriod);
                startOfThisPeriod = endOfThisPeriod;
            }
        }

        /// <summary>
        /// Получение списка всех недель в году
        /// </summary>
        /// <param name="year">Год</param>
        /// <returns></returns>
        public static List<Week> GetAllWeeks(int year)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            DateTime startOfFirstWeek = jan1.AddDays(1 - (int)(jan1.DayOfWeek));
            return Enumerable
                .Range(0, 54)
                .Select(i => new { weekStart = startOfFirstWeek.AddDays(i * 7) })
                .TakeWhile(x => x.weekStart.Year <= jan1.Year)
                .Select(x => new {
                    x.weekStart,
                    weekEnd = x.weekStart.AddDays(4)
                })
                .SkipWhile(x => x.weekEnd < jan1.AddDays(1))
                .Select((x, i) => new Week(x.weekStart, x.weekEnd))
                .ToListEx();
        }

        /// <summary>
        /// Получение списка всех недель в году (Iso)
        /// </summary>
        /// <param name="year">Год</param>
        /// <returns></returns>
        public static List<Week> GetAllWeeksIso(int year)
        {
            return GetAllWeeks(year).WhereEx(x => x.WeekEndDate.Year == year).ToListEx();
        }
    }
}
