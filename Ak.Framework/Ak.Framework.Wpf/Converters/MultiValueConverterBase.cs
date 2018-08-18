using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Ak.Framework.Wpf.Converters
{
    /// <summary>
    /// Базовый класс для конвертации множественных значений
    /// </summary>
    /// <seealso cref="System.Windows.Markup.MarkupExtension" />
    /// <seealso cref="System.Windows.Data.IMultiValueConverter" />
    public abstract class MultiValueConverterBase : MarkupExtension, IMultiValueConverter
    {
        /// <summary>
        /// Конвертация значений
        /// </summary>
        /// <param name="values">Значения</param>
        /// <param name="targetType">Целевой тип</param>
        /// <param name="parameter">Параметр конвертации</param>
        /// <param name="culture">Локаль</param>
        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// Обратная конвертация
        /// </summary>
        /// <param name="value">Значение</param>
        /// <param name="targetTypes">Целевые типы</param>
        /// <param name="parameter">Параметр конвертации</param>
        /// <param name="culture">Локаль</param>
        /// <returns></returns>
        public abstract object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);

        /// <summary>
        /// При применении возвращает объект, передаваемый в качестве значения в целевое свойство
        /// </summary>
        /// <param name="serviceProvider">Провайдер для получения значения</param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
