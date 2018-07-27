using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Ak.Framework.Core.Extensions
{
    /// <summary>
    /// Расширения для List
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Приведение к строковому представлению xml
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="list">Список</param>
        /// <returns></returns>
        public static string ToXmlString<T>(this List<T> list)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<T>));
            MemoryStream ms = new MemoryStream();
            xs.Serialize(ms, list);

            return Encoding.UTF8.GetString(ms.ToArray());
        }

        /// <summary>
        /// Приведение к текстовому списку
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="enumerable">Список</param>
        /// <param name="itemTemplate">Шаблон элемента</param>
        /// <param name="delimeter">Разделитель</param>
        /// <returns></returns>
        public static string ToTextList<T>(this IEnumerable<T> enumerable, string itemTemplate, string delimeter = "<br/>")
        {
            return enumerable == null?
                string.Empty
                : string.Join(delimeter, enumerable.SelectEx(x => x.ToFormatedString(itemTemplate)));
        }

        /// <summary>
        /// Возвращает экземпляр класса T (а не null), если список не содержит элементов
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="list">Список</param>
        /// <returns></returns>
        public static T FirstOrDefaultEx<T>(this List<T> list) where T : new()
        {
            T firstOrDefault = list.FirstOrDefault();

            if (firstOrDefault == null)
                return new T();
            else
                return firstOrDefault;
        }
    }
}
