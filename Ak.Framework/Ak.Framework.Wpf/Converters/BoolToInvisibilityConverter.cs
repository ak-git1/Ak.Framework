using System;
using System.Globalization;
using System.Windows;
using Ak.Framework.Core.Extensions;

namespace Ak.Framework.Wpf.Converters
{
    /// <summary>
    /// Конвертация bool в значение Visibility Collapsed
    /// </summary>
    /// <seealso cref="Ak.Framework.Wpf.Converters.ValueConverterBase" />
    public class BoolToInvisibilityConverter : ValueConverterBase
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
            bool? nullable = value.ToBoolean(null);
            if (nullable == false)
                return Visibility.Visible;
            if (parameter != null && parameter.ToString() == "Collapsed")
                return Visibility.Collapsed;
            return Visibility.Hidden;
        }

        /// <summary>
        /// Обратная конвертация
        /// </summary>
        /// <param name="value">Значение</param>
        /// <param name="targetType">Целевой тип</param>
        /// <param name="parameter">Параметр конвертации</param>
        /// <param name="culture">Локаль</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
