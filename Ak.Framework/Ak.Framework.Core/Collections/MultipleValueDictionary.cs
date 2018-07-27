using System;
using System.Collections.Generic;

namespace Ak.Framework.Core.Collections
{
    /// <summary>
    /// Спаровчник с несколькими значениями
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип значения</typeparam>
    public class MultipleValueDictionary<TKey, TValue> : Dictionary<TKey, List<TValue>>
    {
        #region Публичные методы

        /// <summary>
        /// Добавление значения
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="newItem">Элемент</param>
        public void AddValue(TKey key, TValue newItem)
        {
            EnsureKey(key);
            base[key].Add(newItem);
        }

        /// <summary>
        /// Добавление значений
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="newItems">Добавляемые элементы</param>
        public void AddValues(TKey key, IEnumerable<TValue> newItems)
        {
            EnsureKey(key);
            base[key].AddRange(newItems);
        }

        /// <summary>
        /// Удаление всех элементов
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="match">Критерий удаления</param>
        /// <returns></returns>
        public bool RemoveAll(TKey key, Predicate<TValue> match)
        {
            if (!ContainsKey(key))
                return false;

            base[key].RemoveAll(match);
            if (base[key].Count == 0)
                Remove(key);            

            return true;
        }

        /// <summary>
        /// Удаление элемента
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="value">Тип значения</param>
        /// <returns></returns>
        public bool RemoveValue(TKey key, TValue value)
        {
            if (!ContainsKey(key))
            {
                return false;
            }
            base[key].Remove(value);
            if (base[key].Count == 0)
            {
                Remove(key);
            }
            return true;
        }

        #endregion

        #region Приватные методы

        /// <summary>
        /// Проверка наличия ключа
        /// </summary>
        /// <param name="key">Ключ</param>
        private void EnsureKey(TKey key)
        {
            if (!ContainsKey(key))
            {
                base[key] = new List<TValue>(1);
            }
            else if (base[key] == null)
            {
                base[key] = new List<TValue>(1);
            }
        }

        #endregion
    }
}
