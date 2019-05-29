using System;
using System.Globalization;
using Ak.Framework.Core.Enums;

namespace Ak.Framework.Core.Extensions
{
    /// <summary>
    /// Расширения для даты и времени
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Получение даты и времени без миллисекунд
        /// </summary>
        /// <param name="dateTime">Дата и время</param>
        /// <returns></returns>
        public static DateTime TruncateMilliseconds(this DateTime dateTime)
        {
            return dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.TicksPerMillisecond));
        }

        /// <summary>
        /// Получение даты и времени без секнуд
        /// </summary>
        /// <param name="dateTime">Дата и время</param>
        /// <returns></returns>
        public static DateTime TruncateSeconds(this DateTime dateTime)
        {
            return dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.TicksPerSecond));
        }

        /// <summary>
        /// Получение даты и времени без минут
        /// </summary>
        /// <param name="dateTime">Дата и время</param>
        /// <returns></returns>
        public static DateTime TruncateMinutes(this DateTime dateTime)
        {
            return dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.TicksPerMinute));
        }

        /// <summary>
        /// Получение даты и времени без часов
        /// </summary>
        /// <param name="dateTime">Дата и время</param>
        /// <returns></returns>
        public static DateTime TruncateHours(this DateTime dateTime)
        {
            return dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.TicksPerHour));
        }

        /// <summary>
        /// Получение даты и времени без дней
        /// </summary>
        /// <param name="dateTime">Дата и время</param>
        /// <returns></returns>
        public static DateTime TruncateDays(this DateTime dateTime)
        {
            return dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.TicksPerDay));
        }

        /// <summary>
        /// Установка значения временного интервала
        /// </summary>
        /// <param name="dateTime">Дата и время</param>
        /// <param name="isIntervalAfter">Интервал после</param>
        /// <param name="timeInterval">Временной интервал</param>
        /// <param name="intervalType">Тип интервала времени</param>
        /// <returns></returns>
        public static DateTime SetWithTimeInterval(this DateTime dateTime, bool isIntervalAfter, int timeInterval, TimeIntervalTypes intervalType)
        {
            int timeValue = timeInterval * ((isIntervalAfter) ? 1 : -1);
            switch (intervalType)
            {
                case TimeIntervalTypes.Millisecond:
                    return dateTime.AddMilliseconds(timeValue);

                case TimeIntervalTypes.Second:
                    return dateTime.AddSeconds(timeValue);

                case TimeIntervalTypes.Minute:
                    return dateTime.AddMinutes(timeValue);

                case TimeIntervalTypes.Hour:
                    return dateTime.AddHours(timeValue);

                case TimeIntervalTypes.Day:
                    return dateTime.AddDays(timeValue);

                case TimeIntervalTypes.Month:
                    return dateTime.AddMonths(timeValue);

                case TimeIntervalTypes.Quarter:
                    return dateTime.AddMonths(3 * timeValue);

                case TimeIntervalTypes.Year:
                    return dateTime.AddYears(timeValue);

                default:
                    throw new ArgumentOutOfRangeException("intervalType");
            }
        }

        /// <summary>
        /// Преобразование к дате в строковом формате
        /// </summary>
        /// <param name="dt">Дата и время</param>
        /// <param name="format">Формат</param>
        /// <param name="emptyDateString">Строка для пустой даты</param>
        /// <returns></returns>
        public static string ToDateString(this DateTime? dt, string format = "dd.MM.yyyy", string emptyDateString = "")
        {
            return dt.HasValue ? dt.Value.ToString(format) : emptyDateString;
        }

        /// <summary>
        /// Преобразование к формату длинной строки с русской кодировкой
        /// </summary>
        /// <param name="dateTime">Дата и время</param>
        /// <returns></returns>
        public static string ToRusDateString(this DateTime dateTime)
        {
            return dateTime.ToString("dd MMMM yyyy г.", CultureInfo.GetCultureInfo("ru-RU"));
        }

        /// <summary>
        /// Преобразование к 24-часовому формату времени
        /// </summary>
        /// <param name="dateTime">Дата и время</param>
        /// <returns></returns>
        public static string ToShort24HoursTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("HH:mm");
        }

        /// <summary>
        /// Преобразование к 24-часовому формату времени
        /// </summary>
        /// <param name="dateTime">Дата и время</param>
        /// <param name="emptyDateString">Строка для пустой даты</param>
        /// <returns></returns>
        public static string ToShort24HoursTimeString(this DateTime? dateTime, string emptyDateString = "")
        {
            return dateTime.HasValue ? dateTime.Value.ToString("HH:mm") : emptyDateString; 
        }

        /// <summary>
        /// Получение конца года
        /// </summary>
        /// <param name="date">Дата и время</param>
        /// <returns></returns>
        public static DateTime EndOfYear(this DateTime date)
        {
            return new DateTime(date.Year, 12, 31, 0, 0, 0);
        }

        /// <summary>
        /// Получение конца квартала
        /// </summary>
        /// <param name="date">Дата и время</param>
        /// <returns></returns>
        public static DateTime EndOfQuarter(this DateTime date)
        {
            return date.AddQuarters(1).StartOfQuarter().AddDays(-1).EndOfDay();
        }

        /// <summary>
        /// Получение конца месяца
        /// </summary>
        /// <param name="date">Дата и время</param>
        /// <returns></returns>
        public static DateTime EndOfMonth(this DateTime date)
        {
            return date.AddMonths(1).StartOfMonth().AddDays(-1).EndOfDay();
        }

        /// <summary>
        /// Получение конца недели
        /// </summary>
        /// <param name="date">Дата и время</param>
        /// <returns></returns>
        public static DateTime EndOfWeek(this DateTime date)
        {
            return date.StartOfWeek().AddDays(6);
        }

        /// <summary>
        /// Получение конца дня
        /// </summary>
        /// <param name="date">Дата и время</param>
        /// <returns></returns>
        public static DateTime EndOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }

        /// <summary>
        /// Получение начала года
        /// </summary>
        /// <param name="date">Дата и время</param>
        /// <returns></returns>
        public static DateTime StartOfYear(this DateTime date)
        {
            return new DateTime(date.Year, 1, 1, 0, 0, 0);
        }

        /// <summary>
        /// Получение начала квартала
        /// </summary>
        /// <param name="date">Дата и время</param>
        public static DateTime StartOfQuarter(this DateTime date)
        {
            return new DateTime(date.Year, date.GetQuarter()*3 - 2, 1, 0, 0, 0);
        }

        /// <summary>
        /// Получение начала месяца
        /// </summary>
        /// <param name="date">Дата и время</param>
        /// <returns></returns>
        public static DateTime StartOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1, 0, 0, 0);
        }

        /// <summary>
        /// Получение начала недели
        /// </summary>
        /// <param name="date">Дата и время</param>
        /// <returns></returns>
        public static DateTime StartOfWeek(this DateTime date)
        {
            int weekDayNumber = date.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)date.DayOfWeek;
            int delta = weekDayNumber - (int)DayOfWeek.Monday;
            return date.AddDays(-delta);
        }

        /// <summary>
        /// Получение начала дня
        /// </summary>
        /// <param name="date">Дата и время</param>
        /// <returns></returns>
        public static DateTime StartOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }

        /// <summary>
        /// Приведение даты к строковому представлению для БД
        /// </summary>
        /// <param name="dateTime">Дата и время</param>
        /// <returns></returns>
        public static string ToDatabaseString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        /// <summary>
        /// Получение года
        /// </summary>
        /// <param name="dateTime">Дата и время</param>
        /// <returns></returns>
        public static string Year(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.Year.ToString() : string.Empty;
        }

        /// <summary>
        /// Получение конца дня
        /// </summary>
        /// <param name="date">Дата и время</param>
        /// <returns></returns>
        public static DateTime? EndOfDay(this DateTime? date)
        {
            return date.HasValue ? date.Value.EndOfDay() : (DateTime?)null;
        }

        /// <summary>
        /// Установить временную часть даты
        /// </summary>
        /// <param name="date">Дата</param>
        /// <param name="time">Время</param>
        /// <returns>Дата</returns>
        public static DateTime SetTime(this DateTime date, TimeSpan time)
        {
            return date.StartOfDay().Add(time);
        }

        /// <summary>
        /// Установить временную часть даты
        /// </summary>
        /// <param name="date">Дата</param>
        /// <param name="time">Время</param>
        /// <returns>Дата</returns>
        public static DateTime? SetTime(this DateTime? date, TimeSpan time)
        {
            return date.HasValue ? date.SetTime(time) : null;
        }

        /// <summary>
        /// Добавить к дате количество месяцев по календарным правилам фонда. Учитывается только целое число месяцев.
        /// Времянная часть исходной даты сохраняется
        /// </summary>
        /// <param name="date">Дата</param>
        /// <param name="months">Количество месяцев</param>
        /// <returns>Дата</returns>
        public static DateTime AddMonthsCalendar(this DateTime date, float months)
        {
            //n-е число любого исходного  месяца + m месяцев = n-е число (исходный +m) месяца. 
            //Если n – последнее число месяца, то + m месяцев = последнее число (исходный +m) месяца. 
            //Если (исходный +m) месяц – февраль, то последнему дню февраля соответствуют также 29 и 30 числа исходного месяца.
            //(Последнее выполняется автоматически -  If the resulting day is not a valid day in the resulting month, the last valid day of the resulting month is used. )
            DateTime targetDate = date.AddMonths(months.ToInt32());
            bool isEndOfMonth = date.Day == DateTime.DaysInMonth(date.Year, date.Month);
            return isEndOfMonth ? targetDate.EndOfMonth().SetTime(date.TimeOfDay) : targetDate;
        }

        /// <summary>
        /// Добавить к дате количество месяцев по календарным правилам фонда. Учитывается только целое число месяцев.
        /// Времянная часть исходной даты сохраняется
        /// </summary>
        /// <param name="date">Дата</param>
        /// <param name="months">Количество месяцев</param>
        /// <returns>Дата</returns>
        public static DateTime? AddMonthsCalendar(this DateTime? date, float months)
        {
            return date?.AddMonthsCalendar(months);
        }

        /// <summary>
        /// Проверка что дата меньше заданной (сравнение по дням)
        /// </summary>
        /// <param name="date">Дата</param>
        /// <param name="date2">Дата для проверки</param>
        public static bool IsLess(this DateTime? date, DateTime? date2)
        {
            TimeSpan? delta = date - date2;
            return delta.HasValue && delta.Value.Days < 0;
        }

        /// <summary>
        /// Сдвинуть дату на заданную величину
        /// </summary>
        /// <param name="date">Дата</param>
        /// <param name="durationTimeIntervalType">Тип временного интервала</param>
        /// <param name="shiftValue">Значение сдвига</param>
        public static DateTime Shift(this DateTime date, TimeIntervalTypes? durationTimeIntervalType, int? shiftValue)
        {
            DateTime newDate = date;
            if (!durationTimeIntervalType.HasValue || !shiftValue.HasValue)
                return newDate;

            switch (durationTimeIntervalType.Value)
            {
                case TimeIntervalTypes.Day:
                    newDate = date.AddDays(shiftValue.Value);
                    break;

                case TimeIntervalTypes.Month:
                    newDate = date.AddMonths(shiftValue.Value);
                    break;

                case TimeIntervalTypes.Quarter:
                    newDate = date.AddMonths(3*shiftValue.Value);
                    break;

                case TimeIntervalTypes.Year:
                    newDate = date.AddYears(shiftValue.Value);
                    break;
            }

            return newDate;
        }

        /// <summary>
        /// Возвращает номер квартала по заданной дате
        /// </summary>
        /// <param name="date">Дата</param>
        public static int GetQuarter(this DateTime date)
        {
            if (date.Month >= 4 && date.Month <= 6)
                return 2;
            else if (date.Month >= 7 && date.Month <= 9)
                return 3;
            else if (date.Month >= 10 && date.Month <= 12)
                return 4;
            else
                return 1;
        }

        /// <summary>
        /// Добавить заданное число кварталов к данной дате
        /// </summary>
        /// <param name="date">Дата</param>
        /// <param name="count">Число кварталов</param>
        public static DateTime AddQuarters(this DateTime date, int count)
        {
            return date.AddMonths(3*count);
        }

        /// <summary>
        /// Текущий язык и региональные параметры 
        /// </summary>
        static readonly CultureInfo CurrentCulture = CultureInfo.CurrentCulture;

        /// <summary>
        /// Преобразует "10.09.2015" в "сентябрь 2015"  
        /// </summary>
        /// /// <param name="date">Дата</param>
        public static string ToFormatted(this DateTime date)
        {
            return date.ToString("MMMM yyyy", CurrentCulture).ToLower(CurrentCulture);
        }

        /// <summary>
        /// Возвращает новый объект DateTime, добавляющий заданное число месяцев (в десятичном выражении) к значению данного экземпляра.
        /// </summary>
        /// <param name="dateTime">Дата</param>
        /// <param name="months">Срок в месяцах [Допустимый шаг изменения длительности равен 0,5 месяца]</param>
        /// <returns></returns>
        public static DateTime AddMonths(this DateTime dateTime, decimal? months)
        {
            int totalMonths = months.HasValue ? (int)months.Value : 0;
            int totalWeeks = months.HasValue ? ((int)((months.Value - totalMonths) * 10) != 0 ? 2 : 0) : 0;

            dateTime = dateTime.AddMonths(totalMonths);
            if (totalWeeks != 0)
            {
                return dateTime.AddDays((int)(DateTime.DaysInMonth(dateTime.Year, dateTime.Month) / totalWeeks));
            }

            return dateTime;
        }

        /// <summary>
        /// Получение номера недели
        /// </summary>
        /// <param name="date">Дата</param>
        /// <returns></returns>
        public static int GetIso8601WeekOfYear(this DateTime date)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
                date = date.AddDays(3);

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        /// <summary>
        /// Получение номера недели в месяце
        /// </summary>
        /// <param name="date">Дата</param>
        /// <returns></returns>
        public static int GetWeekNumberOfMonth(this DateTime date)
        {
            date = date.Date;
            DateTime firstMonthDay = new DateTime(date.Year, date.Month, 1);
            DateTime firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            if (firstMonthMonday > date)
            {
                firstMonthDay = firstMonthDay.AddMonths(-1);
                firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            }
            return (date - firstMonthMonday).Days / 7 + 1;
        }
    }
}
