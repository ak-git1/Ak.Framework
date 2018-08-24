using System;
using System.Globalization;
using System.Windows;

namespace Ak.Framework.Wpf.Converters
{
    /// <summary>
    /// Конвертор значения null в Invisibility
    /// </summary>
    /// <seealso cref="Ak.Framework.Wpf.Converters.ValueConverterBase" />
    public sealed class NullToInvisibilityConverter : ValueConverterBase
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
            if (value != null)
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
