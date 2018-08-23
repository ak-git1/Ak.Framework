using System;
using System.Globalization;
using System.Linq;
using static System.Windows.Data.Binding;

namespace Ak.Framework.Wpf.Converters
{
    /// <summary>
    /// Конвертер возвращающий True, если все значения True
    /// </summary>
    /// <seealso cref="Ak.Framework.Wpf.Converters.MultiValueConverterBase" />
    public class AllParametersAreTrueConverter : MultiValueConverterBase
    {
        /// <summary>
        /// Конвертация значений
        /// </summary>
        /// <param name="values">Значения</param>
        /// <param name="targetType">Целевой тип</param>
        /// <param name="parameter">Параметр конвертации</param>
        /// <param name="culture">Локаль</param>
        /// <returns></returns>
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return values.OfType<bool>().All(v => v);
            }
            catch
            {
                return DoNothing;
            }
        }

        /// <summary>
        /// Обратная конвертация
        /// </summary>
        /// <param name="value">Значение</param>
        /// <param name="targetTypes">Целевые типы</param>
        /// <param name="parameter">Параметр конвертации</param>
        /// <param name="culture">Локаль</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">AllParametersAreTrueConverter</exception>
        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("AllParametersAreTrueConverter");
        }
    }
}
