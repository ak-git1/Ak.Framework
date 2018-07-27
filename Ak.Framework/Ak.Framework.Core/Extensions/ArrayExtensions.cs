using System.Collections.Generic;

namespace Ak.Framework.Core.Extensions
{
    /// <summary>
    /// Расширения для массивов
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Преобразование массива в словарь значений
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="array">Словарь</param>
        /// <param name="indexShift">Значение, на которое изменяется ключ</param>
        /// <returns></returns>
        public static Dictionary<int, T> ConvertToDictionary<T>(this T[] array, int indexShift = 0)
        {
            Dictionary<int, T> result = new Dictionary<int, T>();

            if (array != null && array.Length > 0)
                for (int i = 0; i < array.Length; i++)
                    result.Add(i + indexShift, array[i]);

            return result;
        }
    }
}
