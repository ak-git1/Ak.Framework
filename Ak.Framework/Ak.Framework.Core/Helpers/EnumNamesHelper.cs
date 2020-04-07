using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Caching;
using System.Web.UI.WebControls;
using Ak.Framework.Core.Attributes;
using Ak.Framework.Core.Extensions;

namespace Ak.Framework.Core.Helpers
{
    /// <summary>
    /// Класс для получения наименований значений инумератора
    /// </summary>
    public class EnumNamesHelper
    {
        #region Публичные методы

        /// <summary>
        /// Получение описания элемента из инумератора
        /// </summary>
        /// <param name="item">Элемент</param>
        /// <param name="toLower">Привести в нижний регистр</param>
        /// <returns></returns>
        public static string GetDescription(Enum item, bool toLower = false)
        {
            string str = ReadDescription(item);
            return toLower ? str.ToLower() : str;
        }

        /// <summary>
        /// Получение сокращенного описания элемента из инумератора
        /// </summary>
        /// <param name="item">Элемент</param>
        /// <param name="toLower">Привести в нижний регистр</param>
        /// <returns></returns>
        public static string GetShortDescription(Enum item, bool toLower = false)
        {
            string str = ReadFullAndShortDescription(item).Item2;
            return toLower ? str.ToLower() : str;
        }

        /// <summary>
        /// Возвращает список элементов из инумератора
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <returns></returns>
        public static List<T> GetValues<T>()
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Данный метод поддерживает только объекты типа System.Enum");

            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        /// <summary>
        /// Возвращает список элементов из списка
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="list">Список</param>
        /// <returns></returns>
        public static List<ListItem> EnumListToList<T>(IEnumerable<T> list)
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
                throw new ArgumentException("Данный метод поддерживает только объекты типа System.Enum");

            return (from T val in list
                    select GenerateListItem((Enum)Enum.Parse(enumType, val.ToString()))).ToList();
        }

        /// <summary>
        /// Возвращает список элементов из инумератора
        /// </summary>
        /// <param name="typeName">Название типа инумератора</param>
        /// <returns></returns>
        public static List<ListItem> EnumToList(string typeName)
        {
            Type enumType = Type.GetType(typeName);
            if (enumType == null || !enumType.IsEnum)
                throw new ArgumentException("Данный метод поддерживает только объекты типа System.Enum");

            return (from int val in Enum.GetValues(enumType)
                    select GenerateListItem((Enum)Enum.Parse(enumType, val.ToString()))).ToList();
        }

        /// <summary>
        /// Возвращает список элементов из инумератора
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="transformUnknownValueToNull">Преобразовывать 0й элемент перечисления в null</param>
        /// <returns></returns>
        public static List<ListItem> EnumToList<T>(bool transformUnknownValueToNull = false)
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
                throw new ArgumentException("Данный метод поддерживает только объекты типа System.Enum");

            return (from int val in Enum.GetValues(enumType)
                    let item = (Enum)Enum.Parse(enumType, val.ToString(CultureInfo.InvariantCulture))
                    select GenerateListItem(item, transformUnknownValueToNull)).ToList();
        }

        /// <summary>
        /// Возвращает список элементов из инумератора
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="enumList">Список элементов инумероатора</param>
        /// <param name="transformUnknownValueToNull">Преобразовывать 0й элемент перечисления в null</param>
        /// <returns></returns>
        public static List<ListItem> EnumToList<T>(List<T> enumList, bool transformUnknownValueToNull = false)
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
                throw new ArgumentException("Данный метод поддерживает только объекты типа System.Enum");

            return (from int val in Enum.GetValues(enumType).Cast<T>().Where(enumList.Contains)
                    let item = (Enum)Enum.Parse(enumType, val.ToString(CultureInfo.InvariantCulture))
                    select GenerateListItem(item, transformUnknownValueToNull)).ToList();
        }

        /// <summary>
        /// Возвращает список элементов из массива инумераторов
        /// </summary>
        /// <param name="array">Массив инумераторов</param>
        /// <returns></returns>
        public static List<ListItem> EnumArrayToList(Array array)
        {
            return (from Enum e in array select GenerateListItem(e)).ToList();
        }

        /// <summary>
        /// Создание элемента списка
        /// </summary>
        /// <param name="item">Элемент инумератора</param>
        /// <param name="transformUnknownValueToNull">Преобразовывать 0й элемент перечисления в null</param>
        /// <returns></returns>
        public static ListItem GenerateListItem(Enum item, bool transformUnknownValueToNull = false)
        {
            int value = Convert.ToInt32(item);
            int? itemValue = transformUnknownValueToNull && value == 0 ? (int?)null : value;
            return new ListItem(Get(item), itemValue.ToString());
        }

        /// <summary>
        /// Получение названия элемента инумератора
        /// </summary>
        /// <param name="item">Элемент инумератора</param>
        /// <returns></returns>
        public static string Get(Enum item)
        {
            if (item is TraceEventType)
                return Get((TraceEventType)item);

            if (item is HorizontalAlign)
                return Get((HorizontalAlign)item);

            if (item is CacheItemPriority)
                return Get((CacheItemPriority)item);

            return ReadDescription(item);
        }

        /// <summary>
        /// Получение названия элемента инумератора (или значение по умолчанию, если элемент равен null)
        /// </summary>
        /// <param name="item">Элемент инумератора</param>
        /// <param name="defaultValue">Значение по умолчанию</param>
        /// <returns></returns>
        public static string Get(Enum item, string defaultValue)
        {
            return item == null ? defaultValue : Get(item);
        }

        /// <summary>
        /// Получение значения инумератора по описанию
        /// </summary>
        /// <typeparam name="T">Тип инумератора</typeparam>
        /// <param name="description">Описание</param>
        /// <param name="comparisonType">Способ сравнения</param>
        /// <returns></returns>
        public static Enum GetEnumByDescription<T>(string description, StringComparison comparisonType = StringComparison.InvariantCultureIgnoreCase)
        {
            foreach (Enum value in Enum.GetValues(typeof(T)))
                if (GetDescription(value).Equals(description, comparisonType))
                    return value;
            
            return null;
        }

        /// <summary>
        /// Получение значения инумератора по описанию
        /// </summary>
        /// <param name="description">Описание</param>
        /// <param name="sampleEnum">Образец Enum</param>
        /// <param name="comparisonType">Способ сравнения</param>
        /// <returns></returns>
        public static Enum GetEnumByDescription(string description, Enum sampleEnum, StringComparison comparisonType = StringComparison.InvariantCultureIgnoreCase)
        {
            foreach (Enum value in Enum.GetValues(sampleEnum.GetType()))
                if (GetDescription(value).Equals(description, comparisonType))
                    return value;

            return null;
        }

        /// <summary>
        /// Получение значения инумератора по названию
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="enumName">Название значения инумератора</param>
        /// <param name="comparisonType">Способ сравнения</param>
        /// <returns></returns>
        public static T GetEnumByName<T>(string enumName, StringComparison comparisonType = StringComparison.InvariantCultureIgnoreCase)
        {
            return GetValues<T>().FirstOrDefault(x => x.ToStr().Equals(enumName, comparisonType));
        }

        #endregion       

        #region Приватные методы

        /// <summary>
        /// Получение описания элемента из инумератора 
        /// </summary>
        /// <param name="item">Элемент</param>
        /// <returns></returns>
        private static string ReadDescription(Enum item)
        {
            FieldInfo fi = item.GetType().GetField(item.ToString());
            DescriptionAttribute attribute = fi != null ?  (DescriptionAttribute)fi.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() : null;
            return attribute != null ? attribute.Description : string.Empty;
        }

        /// <summary>
        /// Получение полного и сокращенного описаний элемента из инумератора 
        /// </summary>
        /// <param name="item">Элемент</param>
        /// <returns></returns>
        private static Tuple<string, string> ReadFullAndShortDescription(Enum item)
        {
            FieldInfo fi = item.GetType().GetField(item.ToString());
            DescriptionWithShortString attribute = fi != null ? (DescriptionWithShortString)fi.GetCustomAttributes(typeof(DescriptionWithShortString), false).FirstOrDefault() : null;
            return attribute != null
                ? Tuple.Create(attribute.Description, attribute.ShortDescription)
                : Tuple.Create(string.Empty, string.Empty);
        }

        /// <summary>
        /// Получение названия горизонтального выравнивания
        /// </summary>
        /// <param name="align">Выравнивание</param>
        /// <returns></returns>
        private static string Get(HorizontalAlign align)
        {
            switch (align)
            {
                case HorizontalAlign.NotSet:
                    return "Не указано";

                case HorizontalAlign.Left:
                    return "По левому краю";

                case HorizontalAlign.Center:
                    return "По центру";

                case HorizontalAlign.Right:
                    return "По правому краю";

                case HorizontalAlign.Justify:
                    return "По обоим краям";

                default:
                    throw new ArgumentOutOfRangeException("align");
            }
        }

        /// <summary>
        /// Получение названия критичности
        /// </summary>
        /// <param name="severity">Критичность</param>
        private static string Get(TraceEventType severity)
        {
            switch (severity)
            {
                case TraceEventType.Critical:
                    return "Критическая ошибка";

                case TraceEventType.Error:
                    return "Ошибка";

                case TraceEventType.Information:
                    return "Информация";

                case TraceEventType.Resume:
                    return "Возобновление";

                case TraceEventType.Start:
                    return "Старт";

                case TraceEventType.Suspend:
                    return "Приостановка";

                case TraceEventType.Transfer:
                    return "Передача";

                case TraceEventType.Verbose:
                    return "Подробное сообщение";

                case TraceEventType.Warning:
                    return "Предупреждение";

                case TraceEventType.Stop:
                    return "Остановка";

                default:
                    throw new ArgumentOutOfRangeException("severity");
            }
        }

        /// <summary>
        /// Получение названия критичности
        /// </summary>
        /// <param name="severity">Критичность</param>
        private static string Get(CacheItemPriority severity)
        {
            switch (severity)
            {
                case CacheItemPriority.NotRemovable:
                    return "Не обновлять";

                case CacheItemPriority.Default:
                    return "Обновлять";

                default:
                    throw new ArgumentOutOfRangeException("severity");
            }
        }

        #endregion
    }
}
