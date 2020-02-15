using System.Collections.Generic;
using System.Linq;

namespace Ak.Framework.Core.Extensions
{
    /// <summary>
    /// Расширения для словаря
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Получение значения
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="dictionary">Словарь</param>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        public static T GetValue<T>(this Dictionary<string, object> dictionary, string key)
        {
            return dictionary.TryGetValue(key, out object obj2) ? (T)obj2 : default(T);
        }

        /// <summary>
        /// Преобразование словаря в массив
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="dictionary">Словарь</param>
        /// <param name="indexShift">Сдвиг индекса</param>
        /// <returns></returns>
        public static T[] ConvertToArray<T>(this Dictionary<int, T> dictionary, int indexShift = 0)
        {
            int arrayLength = dictionary == null || dictionary.Count == 0 ? 0 : dictionary.Keys.Max();
            T[] result = new T[arrayLength];
            if (arrayLength > 0)
            {
                for (int i = 0; i < arrayLength; i++)
                    result[i] = default(T);
                foreach (var item in dictionary)
                {
                    int index = item.Key + indexShift;
                    if (index >= 0 && index < arrayLength)
                        result[index] = item.Value;
                }
            }
            
            return result;
        }

        /// <summary>
        /// Преобразование в массив
        /// </summary>
        /// <typeparam name="TKey">Тип ключа</typeparam>
        /// <typeparam name="TValue">Тип значения</typeparam>
        /// <param name="dictionary">Словарь</param>
        /// <returns></returns>
        public static TValue[] ConvertToArray<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            TValue[] array = new TValue[dictionary.Keys.Count];

            int index = 0;
            foreach (TKey key in dictionary.Keys)
            {
                array[index] = dictionary[key];
                index++;
            }
            return array;
        }
    }
}
