using System;
using static System.Windows.Data.Binding;

namespace Ak.Framework.Wpf.Converters
{
    /// <summary>
    /// Класс для конвертации значений
    /// </summary>
    public static class ValueConverterHelper
    {
        /// <summary>
        /// Выполнение попытки конвертации
        /// </summary>
        /// <param name="function">Фукция</param>
        /// <returns></returns>
        public static object TryConvert(Func<object> function)
        {
            try
            {
                return function();
            }
            catch (Exception)
            {
                return DoNothing;
            }
        }
    }
}
