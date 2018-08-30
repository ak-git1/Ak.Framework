using System;
using System.Linq;

namespace Ak.Framework.Core.Extensions
{
    /// <summary>
    /// Расширения над стандартными методами целых чисел.
    /// </summary>
    public static class IntegerExtentions
    {
        /// <summary>
        /// Проверка на то, что число находится среди значений инумератора
        /// </summary>
        /// <param name="num">Число</param>
        /// <param name="enums">Массив инумераторов</param>
        /// <returns></returns>
        public static bool IsInEnumsList<T>(this int num, params T[] enums)
        {
            return enums.Any(e => num == Convert.ToInt32(e));
        }

        /// <summary>
        /// Проверка на то, что значение находится среди значений инумератора
        /// </summary>
        /// <param name="num">Значение</param>
        /// <param name="enums">Массив инумераторов</param>
        /// <returns></returns>
        public static bool IsInEnumsList<T>(this T num, params T[] enums)
        {
            return enums != null && enums.Any(e => Convert.ToInt32(num) == Convert.ToInt32(e));
        }

        /// <summary>
        /// Возвращает строку в байтах/килобайтах/мегабайтах
        /// </summary>
        /// <param name="num">Число</param>
        /// <returns></returns>
        public static string ToFileSizeString(this int num)
        {
            if (num < 1024)
                return string.Format("{0} байт", num);
            else
            {
                num /= 1024;
                if (num < 1024)
                    return string.Format("{0} Кбайт", num);
                else
                {
                    num /= 1024;
                    return string.Format("{0} Мбайт", num);
                }
            }
        }

        /// <summary>
        /// Проверить, включены ли значения инумератора (для инумераторов с атрибутом FlagsAttribute)
        /// </summary>
        /// <param name="flagsEnum">Инумератор</param>
        /// <param name="options">Его значения</param>
        /// <returns>Признак</returns>
        public static bool IsFlagsEnabled<T>(this T flagsEnum, params T[] options)
            where T:struct 
        {
            int flagsEnumValue = Convert.ToInt32(flagsEnum);
            if (options == null || options.Length == 0) return true;
            bool result = true;
            foreach (int option in options.Select(o=>Convert.ToInt32(o)))
            {
                result &= ((flagsEnumValue & option) == option);
            }
            return result;
        }

        /// <summary>
        /// Проверить, включены ли значения инумератора (для инумераторов с атрибутом FlagsAttribute)
        /// </summary>
        /// <param name="flagsEnum">Инумератор</param>
        /// <param name="options">Его значения</param>
        /// <returns>Признак</returns>
        public static bool IsFlagsEnabled<T>(this T? flagsEnum, params T[] options) where T : struct
        {
            return IsFlagsEnabled(flagsEnum ?? default(T), options);
        }

        /// <summary>
        /// Приведение к TimeSpan (дни)
        /// </summary>
        /// <param name="days">Дни</param>
        /// <returns></returns>
        public static TimeSpan Days(this int days)
        {
            return TimeSpan.FromDays(days);
        }

        /// <summary>
        /// Приведение к TimeSpan (часы)
        /// </summary>
        /// <param name="hours">Часы</param>
        /// <returns></returns>
        public static TimeSpan Hours(this int hours)
        {
            return TimeSpan.FromHours(hours);
        }

        /// <summary>
        /// Приведение к TimeSpan (миллисекунды)
        /// </summary>
        /// <param name="ms">Миллисекунды</param>
        /// <returns></returns>
        public static TimeSpan Miliseconds(this int ms)
        {
            return TimeSpan.FromMilliseconds(ms);
        }

        /// <summary>
        /// Приведение к TimeSpan (минуты)
        /// </summary>
        /// <param name="minutes">Минуты</param>
        /// <returns></returns>
        public static TimeSpan Minutes(this int minutes)
        {
            return TimeSpan.FromMinutes(minutes);
        }

        /// <summary>
        /// Приведение к TimeSpan (секунды)
        /// </summary>
        /// <param name="seconds">Секунды</param>
        /// <returns></returns>
        public static TimeSpan Seconds(this int seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }

        /// <summary>
        /// Четное
        /// </summary>
        /// <param name="number">Число</param>
        /// <returns></returns>
        public static bool IsEven(this int number)
        {
            return !number.IsOdd();
        }

        /// <summary>
        /// Нечетное
        /// </summary>
        /// <param name="number">Число</param>
        /// <returns></returns>
        public static bool IsOdd(this int number)
        {
            return number % 2 != 0;
        }

        /// <summary>
        /// Преобразование числа в букву
        /// </summary>
        /// <param name="number">Число</param>
        /// <param name="isCaps">Заглавная буква</param>
        /// <returns></returns>
        public static string ToLetter(this int number, bool isCaps = false)
        {
            char c = (char)((isCaps ? 65 : 97) + (number - 1));
            return c.ToString();
        }
    }
}