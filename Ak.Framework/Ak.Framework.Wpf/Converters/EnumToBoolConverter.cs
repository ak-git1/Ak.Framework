using System;
using System.Globalization;
using static System.Windows.Data.Binding;

namespace Ak.Framework.Wpf.Converters
{
    /// <summary>
    /// Преобразует значение типа Enum в bool.
    /// </summary>
    /// <seealso cref="Ak.Framework.Wpf.Converters.ValueConverterBase" />
    public class EnumToBoolConverter : ValueConverterBase
    {
        /// <summary>
        /// Конвертация значений
        /// </summary>
        /// <param name="value">Значение</param>
        /// <param name="targetType">Целевой тип</param>
        /// <param name="parameter">Параметр конвертации</param>
        /// <param name="culture">Локаль</param>
        /// <returns></returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return null;
            }
            string str = value.ToString().ToLower();
            string str2 = parameter.ToString().ToLower();
            return str == str2;
        }

        /// <summary>
        /// Обратная конвертация
        /// </summary>
        /// <param name="value">Значение</param>
        /// <param name="targetType">Целевой тип</param>
        /// <param name="parameter">Параметр конвертации</param>
        /// <param name="culture">Локаль</param>
        /// <returns></returns>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && value.Equals(true) ? Enum.Parse(targetType, parameter.ToString()) : DoNothing;
        }
    }
}
