using System;
using System.Globalization;
using Ak.Framework.Core.Helpers;

namespace Ak.Framework.Wpf.Converters
{
    /// <summary>
    /// Преобразует значение типа Enum в текст.
    /// Возвращает описание объекта (если оно есть) или его текстовое представление.
    /// </summary>
    public sealed class EnumToStringConverter : ValueConverterBase
    {
        /// <summary>
        /// Конвертация значений
        /// </summary>
        /// <param name="value">Значение</param>
        /// <param name="targetType">Целевой тип</param>
        /// <param name="parameter">Параметр конвертации</param>
        /// <param name="culture">Локаль</param>
        /// <returns>Преобразованное значение</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Enum enumValue))
                return null;

            string description = EnumNamesHelper.GetDescription(enumValue);
            return string.IsNullOrWhiteSpace(description) ? enumValue.ToString() : description;
        }

        /// <summary>
        /// Обратная конвертация (не реализована)
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
