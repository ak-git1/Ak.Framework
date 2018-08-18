using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using Ak.Framework.Core.Extensions;

namespace Ak.Framework.Wpf.Converters
{
    /// <summary>
    /// Преобразование строки в DirectoryInfo
    /// </summary>
    /// <seealso cref="Ak.Framework.Wpf.Converters.ValueConverterBase" />
    public sealed class StringToDirectoryInfoConverter : ValueConverterBase
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
            try
            {
                return ((DirectoryInfo)value)?.ToString();
            }
            catch (Exception ex)
            {
                return Binding.DoNothing;
            }
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
            object directoryInfo;
            try
            {
                string str = value.ToStr();
                directoryInfo = str.NotEmpty() ? new DirectoryInfo(str) : null;
            }
            catch (Exception ex)
            {
                directoryInfo = Binding.DoNothing;
            }
            return directoryInfo;
        }
    }
}
