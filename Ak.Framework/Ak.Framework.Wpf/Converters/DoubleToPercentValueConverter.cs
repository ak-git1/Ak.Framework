using System;
using System.Globalization;
using Ak.Framework.Core.Extensions;
using static System.Windows.Data.Binding;

namespace Ak.Framework.Wpf.Converters
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Ak.Framework.Wpf.Converters.ValueConverterBase" />
    public class DoubleToPercentValueConverter : ValueConverterBase
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
            return ValueConverterHelper.TryConvert(delegate {                
                if (value != null && double.TryParse(value.ToString(), out var num))
                    return num * 100.0;

                return DoNothing;
            });
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
            try
            {
                return (double)value / 100.0;
            }
            catch
            {
                return DoNothing;
            }
        }
    }
}
