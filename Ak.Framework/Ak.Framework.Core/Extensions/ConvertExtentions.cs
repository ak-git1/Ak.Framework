using System;
using System.Globalization;
using System.IO;

namespace Ak.Framework.Core.Extensions
{
    /// <summary>
    /// Расширения для конвертации данных
    /// </summary>
    public static class ConvertExtentions
    {
        /// <summary>
        /// Конвертировать в строку.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <returns></returns>
        public static string ToStr(this object obj, string defaultValue = "")
        {
            return obj != null && obj != DBNull.Value ? obj.ToString() : defaultValue;
        }

        /// <summary>
        /// Конвертировать в Int32.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <returns></returns>
        public static int ToInt32(this object obj, int defaultValue = default(int))
        {
            return int.TryParse(obj.ToStr(), out int result) ? result : defaultValue;
        }

        /// <summary>
        /// Конвертировать в Int32.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <returns></returns>
        public static int? ToInt32(this object obj, int? defaultValue)
        {
            return int.TryParse(obj.ToStr(), out int result) ? result : defaultValue;
        }

        /// <summary>
        /// Конвертировать в Int64.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <returns></returns>
        public static long ToInt64(this object obj, long defaultValue = default(long))
        {
            return long.TryParse(obj.ToStr(), out long result) ? result : defaultValue;
        }

        /// <summary>
        /// Конвертировать в Int64.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <returns></returns>
        public static long? ToInt64(this object obj, long? defaultValue)
        {
            return long.TryParse(obj.ToStr(), out long result) ? result : defaultValue;
        }

        /// <summary>
        /// Конвертировать в Double.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <param name="replaceDecimalSeparatorToEnvironmentsDecimalSeparator">Заменить разделитель на разделитель окружения</param>
        /// <returns></returns>
        public static double ToDouble(this object obj, double defaultValue = default(double), bool replaceDecimalSeparatorToEnvironmentsDecimalSeparator = false)
        {
            string str = obj.ToStr();
            if (replaceDecimalSeparatorToEnvironmentsDecimalSeparator)
                str = str.Replace(",",
                    CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                    .Replace(".",
                        CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            return double.TryParse(str, out double result) ? result : defaultValue;
        }

        /// <summary>
        /// Конвертировать в Double.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <param name="replaceDecimalSeparatorToEnvironmentsDecimalSeparator">Заменить разделитель на разделитель окружения</param>
        /// <returns></returns>
        public static double? ToDouble(this object obj, double? defaultValue, bool replaceDecimalSeparatorToEnvironmentsDecimalSeparator = false)
        {
            string str = obj.ToStr();
            if (replaceDecimalSeparatorToEnvironmentsDecimalSeparator)
                str = str.Replace(",",
                    CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                    .Replace(".",
                        CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            return double.TryParse(str, out double result) ? result : defaultValue;
        }

        /// <summary>
        /// Конвертировать в Single.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <returns></returns>
        public static float ToSingle(this object obj, float defaultValue = default(float))
        {
            return ToSingle(obj, null) ?? defaultValue;
        }

        /// <summary>
        /// Конвертировать в Single.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <returns></returns>
        public static float? ToSingle(this object obj, float? defaultValue)
        {
            return float.TryParse(obj.ToStr(), out float result) ? result : defaultValue;
        }

        /// <summary>
        /// Конвертировать в Decimal.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <param name="replaceDecimalSeparatorToEnvironmentsDecimalSeparator">Заменить разделитель на разделитель окружения</param>
        /// <returns></returns>
        public static decimal ToDecimal(this object obj, decimal defaultValue = default(decimal), bool replaceDecimalSeparatorToEnvironmentsDecimalSeparator = false)
        {
            string str = obj.ToStr();

            if (replaceDecimalSeparatorToEnvironmentsDecimalSeparator)
                str = str.Replace(",",
                    CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                    .Replace(".",
                        CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            return decimal.TryParse(str, out decimal result) ? result : defaultValue;
        }

        /// <summary>
        /// Конвертировать в Decimal.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <param name="replaceDecimalSeparatorToEnvironmentsDecimalSeparator">Заменить разделитель на разделитель окружения</param>
        /// <returns></returns>
        public static decimal? ToDecimal(this object obj, decimal? defaultValue, bool replaceDecimalSeparatorToEnvironmentsDecimalSeparator = false)
        {
            string str = obj.ToStr();

            if (replaceDecimalSeparatorToEnvironmentsDecimalSeparator)
                str = str.Replace(",",
                    CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                    .Replace(".",
                        CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            return decimal.TryParse(str, out decimal result) ? result : defaultValue;
        }

        /// <summary>
        /// Конвертировать в Boolean.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <returns></returns>
        public static bool ToBoolean(this object obj, bool defaultValue = default(bool))
        {
            string objStr = obj.ToStr();
            if (objStr.NotEmpty())
            {
                switch (objStr.ToLower())
                {
                    case "нет":
                    case "0":
                    case "false":
                        return false;

                    case "да":
                    case "1":
                    case "true":
                        return true;

                    default:
                        return bool.TryParse(objStr, out bool result) ? result : defaultValue;
                }
            }
            else
                return defaultValue;
        }

        /// <summary>
        /// Конвертировать в Boolean.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <returns></returns>
        public static bool? ToBoolean(this object obj, bool? defaultValue)
        {
            string objStr = obj.ToStr();
            switch (objStr.ToLower())
            {
                case "нет":
                case "0":
                case "false":
                    return false;

                case "да":
                case "1":
                case "true":
                    return true;

                default:
                    return bool.TryParse(objStr, out bool result) ? result : defaultValue;
            }            
        }

        /// <summary>
        /// Конвертировать в Guid.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <returns></returns>
        public static Guid ToGuid(this object obj, Guid defaultValue = default(Guid))
        {
            return ToGuid(obj, null) ?? defaultValue;
        }

        /// <summary>
        /// Конвертировать в Guid.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <returns></returns>
        public static Guid? ToGuid(this object obj, Guid? defaultValue)
        {
            return obj != null && Guid.TryParse(obj.ToStr(), out Guid result) ? result : defaultValue;
        }

        /// <summary>
        /// Конвертировать в DateTime.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object obj, DateTime defaultValue = default(DateTime))
        {
            return DateTime.TryParse(obj.ToStr(), out DateTime result) ? result : defaultValue;
        }

        /// <summary>
        /// Конвертировать в DateTime.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this object obj, DateTime? defaultValue)
        {
            return DateTime.TryParse(obj.ToStr(), out DateTime result) ? result : defaultValue;
        }


        /// <summary>
        /// Конвертировать в TimeSpan.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <returns></returns>
        public static TimeSpan ToTimeSpan(this object obj, TimeSpan defaultValue = default(TimeSpan))
        {
            return ToTimeSpan(obj, null) ?? defaultValue;
        }

        /// <summary>
        /// Конвертировать в TimeSpan?.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <returns></returns>
        public static TimeSpan? ToTimeSpan(this object obj, TimeSpan? defaultValue)
        {
            return TimeSpan.TryParse(obj.ToStr(), out TimeSpan result) ? result : defaultValue;
        }

        /// <summary>
        /// Конвертировать в TimeSpan.
        /// Позволяет устанавливать значения больше 24:00
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <returns></returns>
        public static TimeSpan ToTimeSpanEx(this object obj, TimeSpan defaultValue = default(TimeSpan))
        {
            return ToTimeSpanEx(obj, null) ?? defaultValue;
        }

        /// <summary>
        /// Конвертировать в TimeSpan?.
        /// Позволяет устанавливать значения больше 24:00, например, 25:00:00
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <returns></returns>
        public static TimeSpan? ToTimeSpanEx(this object obj, TimeSpan? defaultValue)
        {
            try
            {
                string[] strs = obj.ToStr().Split(':');
                int hours = 0;
                int minutes = 0;
                int seconds = 0;
                if (strs.Length >= 1)
                    hours = strs[0].ToInt32();
                if (strs.Length >= 2)
                    minutes = strs[1].ToInt32();
                if (strs.Length >= 3)
                    seconds = strs[2].ToInt32();

                return new TimeSpan(hours, minutes, seconds);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Конвертировать в Enum
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <returns></returns>
        public static T ToEnum<T>(this object obj, T defaultValue = default(T)) where T : struct, IConvertible
        {
            return ToEnum(obj, (T?)null) ?? defaultValue;
        }

        /// <summary>
        /// Конвертировать в Enum.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <returns></returns>
        public static T? ToEnum<T>(this object obj, T? defaultValue) where T : struct, IConvertible
        {
            Type enumType = typeof(T);
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Данный метод поддерживает только объекты типа System.Enum");

            string objStr = obj.ToStr();

            if (objStr.NotEmpty())
            {
                if (Enum.IsDefined(enumType, obj) && Enum.TryParse(objStr, out T result))
                    return result;
                if (int.TryParse(objStr, out int intResult) && Enum.IsDefined(enumType, intResult))
                    return (T)Enum.ToObject(enumType, intResult);
            }

            return defaultValue;
        }

        /// <summary>
        /// Приведение к строке
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <param name="trueString">Строка значения true</param>
        /// <param name="falseString">Строка значения false</param>
        /// <returns></returns>
        public static string ToBooleanString(this object obj, string trueString = "Да", string falseString = "Нет")
        {
            return obj.ToBoolean() ? trueString : falseString;
        }

        /// <summary>
        /// Преобразование к дате в строковом формате
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <param name="format">Формат</param>
        /// <param name="emptyDateString">Строка для пустой даты</param>
        /// <returns></returns>
        public static string ToDateString(this object obj, string format = "dd.MM.yyyy", string emptyDateString = "")
        {
            return DateTime.TryParse(obj.ToStr(), out DateTime tempDt) ?
                tempDt.ToString(format)
                : emptyDateString;
        }

        /// <summary>
        /// Преобразование к дате и времени в строковом формате
        /// </summary>
        /// <param name="obj">Дата и время</param>
        /// <param name="format">Формат</param>
        /// <param name="emptyDateString">Строка для пустой даты</param>
        /// <returns></returns>
        public static string ToDateTimeString(this object obj, string format = "dd.MM.yyyy HH:mm", string emptyDateString = "")
        {
            return obj.ToDateString(format, emptyDateString);
        }

        /// <summary>
        /// Преобразование к числу в строковом формате
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <param name="format">Формат</param>
        /// <param name="emptyNumber">Строка для пустого числового значения</param>
        /// <param name="delimeter">Разделитель дробной части</param>
        /// <returns></returns>
        public static string ToNumberString(this object obj, string format = "", string emptyNumber = "", string delimeter = ",")
        {
            return Double.TryParse(obj.ToStr(), out double tempNumber) ?
                tempNumber.ToString(format).Replace(".", delimeter).Replace(",", delimeter)
                : emptyNumber;
        }

        /// <summary>
        /// Преобразование массива байтов в поток
        /// </summary>
        /// <param name="bytes">Массив байтов</param>
        /// <returns></returns>
        public static Stream ToStream(this byte[] bytes)
        {
            return new MemoryStream(bytes);
        }

        /// <summary>
        /// Преобразовать в строку корректную с точки зрения интерпритатора JavaScript
        /// </summary>
        /// <param name="value">Значение</param>
        /// <param name="defaultValue">Значение по умолчанию</param>
        public static string ToJsString(this decimal? value, string defaultValue = null)
        {
            return !value.HasValue
                ? (string.IsNullOrEmpty(defaultValue) ? "0" : defaultValue)
                : value.Value.ToJsString();
        }

        /// <summary>
        /// Преобразовать в строку корректную с точки зрения интерпритатора JavaScript
        /// </summary>
        /// <param name="value">Значение</param>
        public static string ToJsString(this decimal value)
        {
            return value.ToString(CultureInfo.GetCultureInfo("en-US"));
        }

        /// <summary>
        /// Преобразовать в строку корректную с точки зрения интерпритатора JavaScript
        /// </summary>
        /// <param name="value">Значение</param>
        /// <param name="defaultValue">Значение по умолчанию</param>
        public static string ToJsString(this double? value, string defaultValue = null)
        {
            return !value.HasValue
                ? (string.IsNullOrEmpty(defaultValue) ? "0" : defaultValue)
                : value.Value.ToJsString();
        }

        /// <summary>
        /// Преобразовать в строку корректную с точки зрения интерпритатора JavaScript
        /// </summary>
        /// <param name="value">Значение</param>
        public static string ToJsString(this double value)
        {
            return value.ToString(CultureInfo.GetCultureInfo("en-US"));
        }

        /// <summary>
        /// Преобразовать в строку корректную с точки зрения интерпритатора JavaScript
        /// </summary>
        /// <param name="value">Значение</param>
        /// <param name="defaultValue">Значение по умолчанию</param>
        public static string ToJsString(this float? value, string defaultValue = null)
        {
            return !value.HasValue
                ? (string.IsNullOrEmpty(defaultValue) ? "0" : defaultValue)
                : value.Value.ToJsString();
        }

        /// <summary>
        /// Преобразовать в строку корректную с точки зрения интерпритатора JavaScript
        /// </summary>
        /// <param name="value">Значение</param>
        public static string ToJsString(this float value)
        {
            return value.ToString(CultureInfo.GetCultureInfo("en-US"));
        }
    }
}
