using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ak.Framework.Core.Comparers
{
    /// <summary>
    /// Класс для обычного сравнения
    /// </summary>
    public class NaturalSortComparer : IComparer<string>, IDisposable
    {
        #region Переменные

        /// <summary>
        /// Признак того, что сортировка проводится по возрастанию
        /// </summary>
        private readonly bool _isAscending;

        /// <summary>
        /// Таблица сравниваемых элементов
        /// </summary>
        private Dictionary<string, string[]> _table;

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="inAscendingOrder">Признак того, что сортировка проводится по возрастанию</param>
        public NaturalSortComparer(bool inAscendingOrder = true)
        {
            _table = new Dictionary<string, string[]>();
            _isAscending = inAscendingOrder;
        }

        #endregion

        #region Методы

        /// <summary>
        /// Сравнение
        /// </summary>
        /// <param name="x">Объект для сравнения 1</param>
        /// <param name="y">Объект для сравнения 2</param>
        public int Compare(string x, string y)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Деструктор
        /// </summary>
        public void Dispose()
        {
            _table.Clear();
            _table = null;
        }

        /// <summary>
        /// Сравнение частей
        /// </summary>
        /// <param name="left">Левая часть</param>
        /// <param name="right">Правая часть</param>
        /// <returns></returns>
        private static int PartCompare(string left, string right)
        {
            if (!int.TryParse(left, out int num))
                return String.Compare(left, right, StringComparison.Ordinal);

            if (!int.TryParse(right, out int num2))
                return String.Compare(left, right, StringComparison.Ordinal);

            return num.CompareTo(num2);
        }

        /// <summary>
        /// Сравнение
        /// </summary>
        /// <param name="x">Объект для сравнения 1</param>
        /// <param name="y">Объект для сравнения 2</param>
        int IComparer<string>.Compare(string x, string y)
        {
            int num;
            if (x == y)
            {
                return 0;
            }
            if (!_table.TryGetValue(x, out string[] strArray))
            {
                strArray = Regex.Split(x.Replace(" ", ""), "([0-9]+)");
                _table.Add(x, strArray);
            }
            if (!_table.TryGetValue(y, out string[] strArray2))
            {
                strArray2 = Regex.Split(y.Replace(" ", ""), "([0-9]+)");
                _table.Add(y, strArray2);
            }
            for (int i = 0; (i < strArray.Length) && (i < strArray2.Length); i++)
            {
                if (strArray[i] != strArray2[i])
                {
                    num = PartCompare(strArray[i], strArray2[i]);
                    return _isAscending ? num : -num;
                }
            }
            if (strArray2.Length > strArray.Length)
            {
                num = 1;
            }
            else if (strArray.Length > strArray2.Length)
            {
                num = -1;
            }
            else
            {
                num = 0;
            }
            return _isAscending ? num : -num;
        }

        #endregion
    }
}