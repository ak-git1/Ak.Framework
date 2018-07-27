using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Ak.Framework.Core.Extensions
{
    /// <summary>
    /// Расширения для XML
    /// </summary>
    public static class XmlExtensions
    {
        /// <summary>
        /// Получение XML-строки из объекта
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="obj">Объект</param>
        /// <param name="root">Корневой элемент</param>
        /// <returns></returns>
        public static string SerializeToXmlString<T>(this T obj, string root = null)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (XmlWriter w = XmlWriter.Create(ms, new XmlWriterSettings { OmitXmlDeclaration = true }))
                {
                    XmlSerializer serializer = root.NotEmpty() ? new XmlSerializer(typeof(T), new XmlRootAttribute(root)) : new XmlSerializer(typeof(T));
                    serializer.Serialize(w, obj);
                    w.Flush();
                }

                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// Получение объекта из XML-строки
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="str">XML-строка</param>
        /// <param name="root">Корневой элемент</param>
        /// <returns></returns>
        public static T DeserializeFromXmlString<T>(this string str, string root = null)
        {
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(str)))
            using (XmlReader reader = XmlReader.Create(ms))
            {
                XmlSerializer serializer = root.NotEmpty() ? new XmlSerializer(typeof(T), new XmlRootAttribute(root)) : new XmlSerializer(typeof(T));
                T item = (T)serializer.Deserialize(reader);

                return item;
            }
        }
    }
}
