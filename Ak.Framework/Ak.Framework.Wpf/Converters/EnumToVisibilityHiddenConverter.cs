using System;
using System.Globalization;
using System.Windows;

namespace Ak.Framework.Wpf.Converters
{
    /// <summary>
    /// Преобразует значение типа Enum в Visibility
    /// (при совпадении значений возрващает результат Visibility.Hidden)
    /// </summary>
    /// <seealso cref="Ak.Framework.Wpf.Converters.ValueConverterBase" />
    public class EnumToVisibilityHiddenConverter : ValueConverterBase
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
                return Visibility.Hidden;
            
            string str = value.ToString().ToLower();
            string str2 = parameter.ToString().ToLower();
            return str == str2 ? Visibility.Hidden : Visibility.Visible;
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
            throw new NotImplementedException();
        }
    }
}
