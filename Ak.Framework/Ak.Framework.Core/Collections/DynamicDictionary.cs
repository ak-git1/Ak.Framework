using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Ak.Framework.Core.Collections
{
    /// <summary>
    /// Динамический словарь
    /// </summary>
    public class DynamicDictionary : DynamicObject
    {
        #region Переменные

        /// <summary>
        /// Словарь
        /// </summary>
        private readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();

        #endregion

        #region Свойства

        /// <summary>
        /// Количество элементов
        /// </summary>
        public int Count => _dictionary.Count;

        /// <summary>
        /// Значение по ключу
        /// </summary>
        public object this[string key]
        {
            get
            {
                if (!_dictionary.TryGetValue(key, out object obj))
                    throw new ArgumentException("There is an item with such a key in the _dictionary");
                return obj;
            }
        }

        #endregion

        #region Публичные методы

        /// <summary>
        /// Получение списка ключей
        /// </summary>
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _dictionary.Keys;
        }

        /// <summary>
        /// Попытка получения значения по ключу
        /// </summary>
        /// <param name="binder">Объект с ключом</param>
        /// <param name="result">Результат</param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string name = binder.Name;
            return _dictionary.TryGetValue(name, out result);
        }

        /// <summary>
        /// Попытка получения значения
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="value">Значение</param>
        /// <returns></returns>
        public bool TryGetValue(string key, out object value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Попытка установки значения
        /// </summary>
        /// <param name="binder">Объект с ключом</param>
        /// <param name="value">Значение</param>
        /// <returns></returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _dictionary[binder.Name] = value;
            return true;
        }

        #endregion
    }
}