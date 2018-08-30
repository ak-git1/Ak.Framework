using System;
using System.Collections.Generic;
using System.Linq;

namespace Ak.Framework.Core.Extensions
{
    /// <summary>
    /// Расширения над стандартными методами Linq2Object.
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Расширение для метода Select, которое обрабатывает в т.ч. пустые коллекции.
        /// </summary>
        public static IEnumerable<TResult> SelectEx<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return source != null && source.Any() ? source.Select(selector) : new List<TResult>();
        }

        /// <summary>
        /// Итератор, возвращающий индекс текущего значения коллекции
        /// </summary>
        public static void Each<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            int i = 0;
            foreach (T e in source)
                action(e, i++);
        }

        /// <summary>
        /// Расширение для метода Where, которое обрабатывает в т.ч. пустые коллекции.
        /// </summary>
        public static IEnumerable<TSource> WhereEx<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return source != null && source.Any() ? source.Where(predicate) : new List<TSource>();
        }

        /// <summary>
        /// Расширение для метода Aggregate, которое обрабатывает в т.ч. пустые коллекции.
        /// </summary>
        public static TSource AggregateEx<TSource>(this IEnumerable<TSource> source, Func<TSource,TSource,TSource> func)
        {
            return source != null && source.Any() ? source.Aggregate(func) : default(TSource);
        }

        /// <summary>
        /// Расширение для метода ToList, которое обрабатывает в т.ч. пустые коллекции.
        /// </summary>
        public static List<T> ToListEx<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null ? new List<T>() : enumerable.ToList();
        }

        /// <summary>
        /// Расширение для метода Average, которое обрабатывает в т.ч. пустые коллекции.
        /// </summary>
        public static double AverageEx<T>(this IEnumerable<T> enumerable, Func<T, double> selector,  double defaultValue = 0)
        {
            return enumerable != null && enumerable.Any() ? enumerable.Average(selector) : defaultValue;
        }

        /// <summary>
        /// Расширение для метода Average, которое обрабатывает в т.ч. пустые коллекции.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source">Входящий список</param>
        /// <param name="selector">Функция преобразования, применяемая к каждому элементу</param>
        /// <param name="defaultValue">Значение по умолчанию</param>
        /// <returns></returns>
        public static decimal AverageEx<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector, decimal defaultValue = 0)
        {
            return source != null && source.Any() ? source.Average(selector) : defaultValue;
        }

        /// <summary>
        /// Клонирование списка
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="list">Список</param>
        /// <returns></returns>
        public static List<T> Clone<T>(this List<T> list) where T : ICloneable
        {
            return list.Select(item => (T)item.Clone()).ToList();
        }

        /// <summary>
        /// Получение предыдущего значения в списке или значения по умолчанию
        /// </summary>
        /// <typeparam name="TSource">Тип</typeparam>
        /// <param name="source">Список</param>
        /// <param name="match">Сравнение</param>
        /// <returns></returns>
        public static TSource PreviousOrDefault<TSource>(this IEnumerable<TSource> source, Predicate<TSource> match)
        {
            IList<TSource> enumerable = source as IList<TSource> ?? source.ToList();
            int i = enumerable.ToList().FindIndex(match);
            return i > 0 && i < enumerable.Count ? enumerable[i-1] : default(TSource);
        }

        /// <summary>
        /// Получение следующего значения в списке или значения по умолчанию 
        /// </summary>
        /// <returns></returns>
        public static TSource NextOrDefault<TSource>(this IEnumerable<TSource> source, Predicate<TSource> match)
        {
            IList<TSource> enumerable = source as IList<TSource> ?? source.ToList();
            int i = enumerable.ToList().FindIndex(match);
            return i >= 0 && i < enumerable.Count - 1 ? enumerable[i + 1] : default(TSource);
        }

        /// <summary>
        /// Проверка на то, что элемент является последним
        /// </summary>
        /// <typeparam name="T">Тим</typeparam>
        /// <param name="items">Список</param>
        /// <param name="item">Элемент</param>
        /// <returns></returns>
        public static bool IsLast<T>(this IEnumerable<T> items, T item)
        {
            T last = items.LastOrDefault();
            if (last == null)
                return false;
            else
                return item.Equals(last);
        }

        /// <summary>
        /// Проверка на то, что элемент является первым
        /// </summary>
        /// <typeparam name="T">Тим</typeparam>
        /// <param name="items">Список</param>
        /// <param name="item">Элемент</param>
        /// <returns></returns>
        public static bool IsFirst<T>(this IEnumerable<T> items, T item)
        {
            T first = items.FirstOrDefault();
            if (first == null)
                return false;
            else
                return item.Equals(first);
        }

        /// <summary>
        /// Проверка на то, что элемент является первым или последним
        /// </summary>
        /// <typeparam name="T">Тим</typeparam>
        /// <param name="items">Список</param>
        /// <param name="item">Элемент</param>
        /// <returns></returns>
        public static bool IsFirstOrLast<T>(this IEnumerable<T> items, T item)
        {
            return items.IsFirst(item) || items.IsLast(item);
        }

        /// <summary>
        /// Преобразование элемента в последовательность
        /// </summary>
        /// <typeparam name="T">Тип элемента</typeparam>
        /// <param name="elem">Элемент</param>
        /// <returns></returns>
        public static IEnumerable<T> ToSequence<T>(this T elem)
        {
            return Enumerable.Repeat(elem, 1);
        }

        /// <summary>
        /// Создать последовательность из одного элемента
        /// </summary>
        /// <typeparam name="T">Тип элемента</typeparam>
        /// <param name="element">Элемент</param>
        /// <returns></returns>
        public static IEnumerable<T> One<T>(this T element)
        {
            return new[] { element };
        }

        /// <summary>
        /// Добавление диапазона
        /// </summary>
        /// <typeparam name="TItem">Тип</typeparam>
        /// <param name="list">Список</param>
        /// <param name="items">Элементы для добавления</param>
        public static void AddRange<TItem>(this ICollection<TItem> list, IEnumerable<TItem> items)
        {
            foreach (TItem item in items)
                list.Add(item);
        }

        /// <summary>
        /// Добавление элемента
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="list">Список</param>
        /// <param name="item">Элемент для добавления</param>
        /// <returns></returns>
        public static IList<T> Append<T>(this IList<T> list, T item)
        {
            list.Add(item);
            return list;
        }

        /// <summary>
        /// Выполнение цикличного запуска
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="enumerable">Список</param>
        /// <param name="action">Метод</param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            Helpers.ThrowHelper.CheckNotNull(action, "action");
            if (enumerable != null)
                foreach (T item in enumerable)
                    action(item);
        }

        /// <summary>
        /// Выполнение цикличного запуска
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="enumerable">Список</param>
        /// <param name="action">Метод</param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
        {
            foreach (var variable in enumerable.Select((T item, int index) => new { item = item, index = index }))
                action(variable.item, variable.index);
        }

        /// <summary>
        /// Проверка на то, что список пуст
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="enumerable">Список</param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.Any();
        }

        /// <summary>
        /// Сортировка
        /// </summary>
        /// <typeparam name="TItem">Тип</typeparam>
        /// <param name="enumerable">Список</param>
        /// <param name="comparerFunc">Метод для сравнения</param>
        /// <param name="ascending">Признак того, что сортировка идет по возрастанию</param>
        /// <returns></returns>
        public static IEnumerable<TItem> Sort<TItem>(this IEnumerable<TItem> enumerable, Func<TItem, TItem, int> comparerFunc, bool ascending = true)
        {
            TItem[] array = enumerable.ToArray();
            Array.Sort(array, new ActionComparer<TItem>(comparerFunc));
            if (!ascending)
                Array.Reverse(array);

            return array;
        }

        /// <summary>
        /// Класс для сравнения элементов
        /// </summary>
        /// <typeparam name="TItem">Тип элемента для сравнения</typeparam>
        private sealed class ActionComparer<TItem> : IComparer<TItem>
        {
            #region

            /// <summary>
            /// Метод для сравнения
            /// </summary>
            private readonly Func<TItem, TItem, int> _comparer;

            #endregion

            #region Конструктор

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="comparer">Метод для сравнения</param>
            public ActionComparer(Func<TItem, TItem, int> comparer)
            {
                _comparer = comparer;
            }

            #endregion

            #region Методы

            /// <summary>
            /// Сравнение
            /// </summary>
            /// <param name="x">Объект для сравнения 1</param>
            /// <param name="y">Объект для сравнения 2</param>
            public int Compare(TItem x, TItem y)
            {
                return _comparer(x, y);
            }

            #endregion
        }
    }
}
