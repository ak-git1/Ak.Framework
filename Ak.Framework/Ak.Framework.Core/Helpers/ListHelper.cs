using System;
using System.Collections.Generic;
using System.Linq;

namespace Ak.Framework.Core.Helpers
{
    /// <summary>
    /// Класс для работы со списками
    /// </summary>
    public static class ListHelper
    {
        /// <summary>
        /// Разделение списка на подсписки
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="list">Список</param>
        /// <param name="subListSize">Размер подсписка</param>
        /// <returns></returns>
        public static IEnumerable<List<T>> SplitList<T>(List<T> list, int subListSize)
        {
            for (int i = 0; i < list.Count; i += subListSize)
                yield return list.GetRange(i, Math.Min(subListSize, list.Count - i));
        }

        /// <summary>
        /// Получение всех комбинаций из элементов
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="list">Список элементов</param>
        /// <returns></returns>
        public static List<List<T>> GetAllCombinations<T>(List<T> list)
        {
            List<List<T>> result = new List<List<T>> { new List<T>() };

            result.Last().Add(list.First());
            if (list.Count == 1)
                return result;

            List<List<T>> tailCombos = GetAllCombinations(list.Skip(1).ToList());
            tailCombos.ForEach(combo =>
            {
                result.Add(new List<T>(combo));
                combo.Add(list.First());
                result.Add(new List<T>(combo));
            });

            return result;
        }
    }
}
