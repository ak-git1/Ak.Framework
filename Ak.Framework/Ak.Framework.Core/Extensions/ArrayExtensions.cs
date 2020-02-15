using System;
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

        /// <summary>
        /// Преобразование массива в словарь с ключами из индексов
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="array">Массив</param>
        /// <param name="itemCondition">Условие</param>
        /// <returns></returns>
        public static Dictionary<int, T> ArrayToDictionaryWithIndexInKey<T>(this T[] array, Predicate<T> itemCondition = null)
        {
            Dictionary<int, T> output = new Dictionary<int, T>();
            for (int i = 0; i < array.Length; i++)
            {
                T item = array[i];
                if (itemCondition == null)
                    output.Add(i, item);
                else if (itemCondition(item))
                    output.Add(i, item);
            }
            return output;
        }
    }
}
