using System;
using System.Collections.Generic;

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
    }
}
