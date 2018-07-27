using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace Ak.Framework.Core.Helpers
{
    /// <summary>
    /// Класс для сериализации списков
    /// </summary>
    public static class ListSerializationHelper
    {
        /// <summary>
        /// Сериализовать коллекцию через XmlWriter
        /// </summary>
        /// <typeparam name="T">Тип элементов коллекции</typeparam>
        /// <param name="collection">Коллекция</param>
        /// <param name="writer">Генератор xml</param>
        public static void WriteXml<T>(IList<T> collection, XmlWriter writer)
        {
            if (collection == null || !collection.Any()) return;

            int position = 0;
            foreach (T element in collection)
            {
                string xmlElementName = FormElementName(position++);
                writer.WriteStartElement(xmlElementName);
                if (element == null)
                {
                    writer.WriteAttributeString("type", "null");
                }
                else
                {
                    writer.WriteAttributeString("type", element.GetType().AssemblyQualifiedName);
                    XmlSerializer serializer = new XmlSerializer(element.GetType());
                    serializer.Serialize(writer, element);
                }
                writer.WriteEndElement();
            }
        }

        /// <summary>
        /// Десериализовать коллекцию из xml
        /// </summary>
        /// <typeparam name="T">Тип элементов коллекции</typeparam>
        /// <param name="collection">Коллекция</param>
        /// <param name="reader">Читалка xml</param>
        public static void ReadXml<T>(IList<T> collection, XmlReader reader)
        {
            int position = 0;
            while (reader.ReadToFollowing(FormElementName(position++)))
            {
                reader.MoveToAttribute("type");
                string type = reader.Value;
                T element = default(T);
                if (type != "null")
                {
                    Type runtimeType = Type.GetType(type);
                    reader.Read();
                    XmlSerializer serializer = new XmlSerializer(runtimeType);
                    element = (T)serializer.Deserialize(reader);
                }
                collection.Add(element);
            }
        }

        /// <summary>
        /// Формирование имени элемента
        /// </summary>
        /// <param name="elementPosition">Позиция элемента</param>
        /// <returns>Имя элемента</returns>
        private static string FormElementName(int elementPosition)
        {
            return $"e{elementPosition}";
        }
    }
}
