using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace Ak.Framework.Core.Helpers
{
    /// <summary>
    /// Класс для работы с Xml
    /// </summary>
    public class XmlHelper
    {
        /// <summary>
        /// Сериализация строки
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="value">Сериализуемый объект</param>
        /// <returns></returns>
        public static string Serialize<T>(T value)
        {
            return Serialize(value, Encoding.UTF8);
        }

        /// <summary>
        /// Сериализация строки
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="value">Сериализуемый объект</param>
        /// <param name="encoding">Кодировка</param>
        /// <param name="stripIllegalCharacters">Удалять запрещенные символы?</param>
        /// <param name="xmlVersion">Версия XML</param>
        /// <returns></returns>
        public static string Serialize<T>(T value, Encoding encoding, bool stripIllegalCharacters = false, string xmlVersion = "1.1")
        {
            if (value == null)
                return null;

            string retval;

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            using (MemoryStream memoryStream = new MemoryStream())
            using (XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, encoding))
            {
                xmlTextWriter.Formatting = Formatting.None;
                xmlSerializer.Serialize(xmlTextWriter, value);
                retval = encoding.GetString(memoryStream.ToArray());
            }

            if (!stripIllegalCharacters)
                return retval;

            switch (xmlVersion)
            {
                case "1.0":
                    return
                        new Regex(@"&#x(((10?|[2-F])FFF[EF]|FDD[0-9A-F]|7F|8[0-46-9A-F]9[0-9A-F])|(FFF[EF]));",
                            RegexOptions.IgnoreCase).Replace(retval, string.Empty);
                case "1.1":
                    return
                        new Regex(@"&#x(((10?|[2-F])FFF[EF]|FDD[0-9A-F]|[19][0-9A-F]|7F|8[0-46-9A-F]|0?[1-8BCEF])|FFF[EF]);",
                            RegexOptions.IgnoreCase).Replace(retval, string.Empty);
                default:
                    return retval;
            }
        }

        /// <summary>
        /// Десериализация Xml-строки в объект
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="xml">Xml-строка</param>
        /// <returns></returns>
        public static T Deserialize<T>(string xml)
        {

            if (string.IsNullOrEmpty(xml))
                return default(T);

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlReaderSettings settings = new XmlReaderSettings();

            using (StringReader textReader = new StringReader(xml))
            {
                using (XmlReader xmlReader = XmlReader.Create(textReader, settings))
                {
                    return (T)serializer.Deserialize(xmlReader);
                }
            }
        }

        /// <summary>
        /// Генерация Xml документа
        /// </summary>
        /// <param name="xml">Xml</param>
        /// <returns></returns>
        public static XmlDocument GenerateDocument(string xml)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                return doc;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Генерация Xml документа из существующего файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns></returns>
        public static XmlDocument OpenDocument(string path)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                return doc;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Генерации Xml документа из потока
        /// </summary>
        /// <param name="stream">Поток</param>
        /// <returns></returns>
        public static XmlDocument OpenDocument(StringReader stream)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(stream);

                return doc;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Получение аттрибутов корневого узла Xml документа
        /// </summary>
        /// <param name="xmlDoc">XmlDocument объект</param>
        /// <returns>XmlAttributeCollection объект с аттрибутами узла</returns>
        public static XmlAttributeCollection GetAttributes(XmlDocument xmlDoc)
        {
            try
            {
                XmlNode node = xmlDoc.FirstChild;
                return node.Attributes;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Получение аттрибутов Xml документа
        /// </summary>
        /// <param name="xmlText">Строка Xml</param>
        /// <returns>XmlAttributeCollection объект с аттрибутами узла</returns>
        public static XmlAttributeCollection GetAttributes(string xmlText)
        {
            XmlDocument xmlDoc = GenerateDocument(xmlText);
            if (xmlDoc != null)
                return GetAttributes(xmlDoc);

            return null;
        }

        /// <summary>
        /// Получение определенного аттрибута Xml документа
        /// </summary>
        /// <param name="xmlText">Xml документ в виде текста</param>
        /// <param name="attributeName">Название аттрибута</param>
        /// <returns></returns>
        public static string GetAttribute(string xmlText, string attributeName)
        {
            try
            {
                XmlAttributeCollection attr = GetAttributes(xmlText);
                return attr[attributeName].Value;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Функция удаления аттрибутов Xml документа
        /// </summary>
        /// <param name="xmlText">Xml документ</param>
        /// <param name="attributeName">Список аттрибутов</param>
        public static string RemoveAttributes(string xmlText, string[] attributeName)
        {
            try
            {
                XmlDocument xmlDoc = GenerateDocument(xmlText);

                foreach (string attr in attributeName)
                {
                    if ((XmlAttribute)xmlDoc.FirstChild.Attributes.GetNamedItem(attr) != null)
                        xmlDoc.FirstChild.Attributes.RemoveNamedItem(attr);
                }

                return xmlDoc.OuterXml;
            }
            catch
            {
                return xmlText;
            }
        }

        /// <summary>
        /// Получение Xml-узлов по названию
        /// </summary>
        /// <param name="xmlString">Xml</param>
        /// <param name="xpath">Xpath</param>
        /// <returns></returns>
        public static XmlNodeList GetXmlNodes(string xmlString, string xpath)
        {
            XmlDocument xml = GenerateDocument(xmlString);
            return xml.SelectNodes(xpath);
        }

        /// <summary>
        /// Получение Xml-узлов по названию
        /// </summary>
        /// <param name="xmlString">Xml</param>
        /// <param name="xpath">Xpath</param>
        /// <returns></returns>
        public static XmlNode GetFirstOrDefaultXmlNode(string xmlString, string xpath)
        {
            XmlDocument xml = GenerateDocument(xmlString);
            return xml.SelectNodes(xpath).Cast<XmlNode>().FirstOrDefault();
        }
    }
}
