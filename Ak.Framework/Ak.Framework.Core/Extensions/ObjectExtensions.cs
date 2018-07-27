using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml.Linq;

namespace Ak.Framework.Core.Extensions
{
    /// <summary>
    /// Расширения для объекта
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Приведение типов без проверки
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="obj">Объект</param>
        /// <returns></returns>
        public static T Cast<T>(this object obj)
        {
            return (T)obj;
        }

        /// <summary>
        /// Получение форматированной строки
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <param name="format">Формат</param>
        /// <returns></returns>
        public static string ToFormatedString(this object obj, string format)
        {
            string result = format;

            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                string stringForReplace = string.Format("{{{0}}}", prop.Name);
                if (result.Contains(stringForReplace))
                {
                    object value = prop.GetValue(obj, null);
                    result = result.Replace(stringForReplace, value != null ? value.ToString() : string.Empty);
                }
            }

            PropertyInfo property = obj.GetType().GetProperty("Deleted");
            if (property == null)
            {
                return result;
            }
            else
            {
                if (property.GetValue(obj).ToString().Equals("True"))
                    result = string.Format("{0} (удалено)", result);
            }

            return result;
        }

        /// <summary>
        /// Получение значения свойства по названию и атрибуту
        /// </summary>
        /// <typeparam name="T">Атрибут свойства</typeparam>
        /// <param name="obj">Объект</param>
        /// <param name="propertyName">Имя свойства</param>
        /// <returns></returns>
        public static object GetValueByPropertyName<T>(this object obj, string propertyName)
        {
            if (propertyName.NotEmpty())
            {
                string[] propertyNames = propertyName.Split('.');
                object propertyObj = null;

                PropertyInfo[] properties = obj.GetType().GetProperties().Where(pi => pi.GetCustomAttributes(typeof(T), false).Length > 0).ToArray();
                propertyObj = (from property in properties
                               where string.Equals(property.Name, propertyNames[0])
                               select property.GetValue(obj, null)).FirstOrDefault();

                if (propertyNames.Count() > 1)
                {
                    propertyName = propertyName.Substring(propertyNames[0].Length + 1);
                    return propertyObj.GetValueByPropertyName(propertyName);
                }
                else
                    return propertyObj;
            }
            else
                return null;
        }

        /// <summary>
        /// Получение значения свойства по названию и атрибуту
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <param name="propertyName">Имя свойства</param>
        /// <returns></returns>
        public static object GetValueByPropertyName(this object obj, string propertyName)
        {
            if (obj != null && propertyName.NotEmpty())
            {
                string[] propertyNames = propertyName.Split('.');
                object propertyObj = null;

                PropertyInfo[] properties = obj.GetType().GetProperties().ToArray();                
                propertyObj = ( from property in properties
                                where string.Equals(property.Name, propertyNames[0])
                                select property.GetValue(obj, null)).FirstOrDefault();
                if (propertyNames.Length > 1)
                {
                    propertyName = propertyName.Substring(propertyNames[0].Length + 1);
                    return propertyObj.GetValueByPropertyName(propertyName);
                }
                else
                    return propertyObj;
            }
            else
                return null;
        }

        /// <summary>
        /// Получение значения метода по его названию и входным параметрам
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <param name="methodNameWithParameters">Строка с именем метода и его параметрами в круглых скобках</param>
        public static object GetValueByMethodName(this object obj, string methodNameWithParameters)
        {
            if (obj != null && methodNameWithParameters.NotEmpty())
            {
                string[] methodStrings = methodNameWithParameters.Split("(,)".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                object methodValueObj = null;

                MethodInfo[] methods = obj.GetType().GetMethods().ToArray();
                MethodInfo method = methods.FirstOrDefault(m => string.Equals(m.Name, methodStrings[0].Trim()));
                if (method != null)
                {
                    object[] parametersValues = null;
                    ParameterInfo[] parameters = method.GetParameters();
                    if (parameters.Length != methodStrings.Length - 1)
                        return null;

                    if (parameters.Length > 0)
                    {
                        parametersValues = new object[parameters.Length];
                        foreach (ParameterInfo parameter in parameters)
                        {
                            Type type = parameter.ParameterType;
                            string strParameterValue = methodStrings[parameter.Position + 1];
                            parametersValues[parameter.Position] = Convert.ChangeType(strParameterValue, type);
                        }
                    }

                    methodValueObj = method.Invoke(obj, parametersValues);
                    //if (method.ReturnType.ToString().Equals("System.Single"))
                    //{
                    //    decimal d = Decimal.Parse(methodValueObj.ToString(), NumberStyles.Float);
                    //    //float amount;
                    //    //float.TryParse(methodValueObj, NumberStyles.AllowThousands, null, out amount);
                    //    return (float)d;
                    //}
                }

                return methodValueObj;
            }
            else
                return null;
        }

        /// <summary>
        /// Сохранение значения в свойство по названию и атрибуту
        /// </summary>
        /// <typeparam name="T">Атрибут свойства</typeparam>
        /// <param name="obj">Объект</param>
        /// <param name="propertyName">Имя свойства</param>
        /// <param name="value">Значение</param>
        public static void SetValueByPropertyName<T>(this object obj, string propertyName, object value)
        {
            if (propertyName.NotEmpty())
            {
                PropertyInfo[] properties = obj.GetType().GetProperties().Where(pi => pi.GetCustomAttributes(typeof(T), false).Length > 0).ToArray();
                foreach (PropertyInfo property in properties)
                    if (string.Equals(property.Name, propertyName))
                    {
                        Type t = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                        object propertyValue = (value == null) ? null : Convert.ChangeType(value, t);
                        property.SetValue(obj, propertyValue, null);
                        return;
                    }
            }
        }

        /// <summary>
        /// Сохранение значения в свойство по названию и атрибуту
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <param name="propertyName">Имя свойства</param>
        /// <param name="value">Значение</param>
        public static void SetValueByPropertyName(this object obj, string propertyName, object value)
        {
            if (propertyName.NotEmpty())
            {
                PropertyInfo[] properties = obj.GetType().GetProperties().ToArray();
                foreach (PropertyInfo property in properties)
                    if (string.Equals(property.Name, propertyName))
                    {
                        Type t = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                        property.SetValue(obj, (value == null) ? null : Convert.ChangeType(value, t), null);
                        return;
                    }
            }
        }

        /// <summary>
        /// Получение атрибута
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="obj">Объект</param>
        /// <returns></returns>
        public static T GetAttribute<T>(this object obj) where T : Attribute
        {
            if (obj == null) throw new ArgumentNullException("obj");
            Type type = obj.GetType();
            return type.IsEnum
                ? obj.GetEnumValueAttribute<T>()
                : obj.GetType().GetCustomAttribute(typeof (T), true) as T;
        }

        /// <summary>
        /// Получение атрибута элемента перечисления
        /// </summary>
        /// <typeparam name="T">Тип атрибута</typeparam>
        /// <param name="enumValue">Значение перечисления</param>
        /// <returns>Значение атрибута</returns>
        private static T GetEnumValueAttribute<T>(this object enumValue) where T : Attribute
        {
            Type type = enumValue.GetType();
            FieldInfo fi = type.GetField(enumValue.ToString());
            T attribute = (T)fi.GetCustomAttributes(typeof(T), false).FirstOrDefault();
            return attribute;
        }

        /// <summary>
        /// Проверка наличия объекта в XML-строке значений
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <param name="objectsXml">XML-строка, содержащая объекты</param>
        /// <param name="tag">XML-тег, содержащий объект</param>
        /// <returns></returns>
        public static bool? IsInXmlString(this object obj, string objectsXml, string tag = "int")
        {
            if (objectsXml.NotEmpty())
            {
                string[] expertisesStatesIds = objectsXml.ToArrayFromXml(tag);
                if (expertisesStatesIds != null && expertisesStatesIds.Length > 0)
                    return expertisesStatesIds.Contains(obj.ToStr());
            }

            return null;
        }

        /// <summary>
        /// Возвращает NULL, если значения равны
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="obj">Объект</param>
        /// <param name="value">Значение</param>
        /// <returns></returns>
        public static T? NullIf<T>(this T obj, T value) where T : struct
        {
            if (obj.Equals(value))
                return null;
            return obj;
        }

        /// <summary>
        /// Возвращает XML представление свойств
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <param name="propertiesNames">Список названий свойств</param>
        /// <returns>XML представление свойств</returns>
        public static XDocument GetPropertiesInXmlFormat(this object obj, List<string> propertiesNames)
        {
            XDocument document = new XDocument();
            XElement properties = new XElement("properties");

            foreach (string propertyName in propertiesNames)
            {
                object property = obj.GetType().GetProperty(propertyName).GetValue(obj, null);
                if (property != null)
                    properties.Add(new XElement(propertyName, property.ToString()));
            }

            document.Add(properties);
            return document;
        }

        /// <summary>
        /// Возвращает XML представление свойств
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <param name="props">Словарь названий свойств и их алиасов для XML тегов (key - название свйоства, value - название тега в XML, в который будет записано значение свайства)</param>
        /// <returns>XML представление свойств</returns>
        public static XDocument GetPropertiesInXmlFormat(this object obj, Dictionary<string, string> props)
        {
            XDocument document = new XDocument();
            XElement properties = new XElement("properties");

            foreach (KeyValuePair<string, string> prop in props)
            {
                object property = obj.GetType().GetProperty(prop.Key).GetValue(obj, null);
                if (property != null)
                    properties.Add(new XElement(prop.Value, property.ToString()));
            }

            document.Add(properties);
            return document;
        }

        /// <summary>
        /// Объект является пустым списком
        /// </summary>
        /// <param name="obj">Объект</param>
        public static bool IsEmptyList(this object obj)
        {
            if (!obj.IsList())
                return false;

            IList list = obj as IList;
            return list != null && list.Count == 0;
        }

        /// <summary>
        /// Объект является списком
        /// </summary>
        /// <param name="obj">Объект</param>
        public static bool IsList(this object obj)
        {
            return obj is IList && obj.GetType().IsGenericType;
        }

        /// <summary>
        /// Переводит число с плавающей точкой в экспоненциоальном 
        /// формате (типа 1.2345E+05) в читабельную строку
        /// </summary>
        /// <param name="obj">Объект</param>
        public static string ToLongFloatString(this object obj)
        {
            string str = obj.ToStr().ToUpper();
            
            if (!str.Contains("E"))
                return str;

            bool negativeNumber = false;

            if (str[0] == '-')
            {
                str = str.Remove(0, 1);
                negativeNumber = true;
            }

            string sep = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            char decSeparator = sep.ToCharArray()[0];

            string[] exponentParts = str.Trim().Split('E');
            string[] decimalParts = exponentParts[0].Split(decSeparator);

            if (decimalParts.Length == 1) 
                decimalParts = new [] { exponentParts[0], "0" };

            int exponentValue = int.Parse(exponentParts[1]);

            string newNumber = string.Concat(decimalParts[0], decimalParts[1]);

            string result;

            if (exponentValue > 0)
            {
                result = string.Concat(newNumber,
                                       GetZeros(exponentValue - decimalParts[1].Length));
            }
            else 
            {
                result = string.Concat("0",
                                       decSeparator,
                                       GetZeros(exponentValue + decimalParts[0].Length),
                                       newNumber);

                result = result.TrimEnd('0');
            }

            if (negativeNumber)
                result = "-" + result;

            return result;
        }

        /// <summary>
        /// Получить строку нулей
        /// </summary>
        /// <param name="zeroCount">Число нулей</param>
        private static string GetZeros(int zeroCount)
        {
            if (zeroCount < 0)
                zeroCount = Math.Abs(zeroCount);

            return string.Empty.PadLeft(zeroCount, '0');
        }

        /// <summary>
        /// Выполнение метода, если значение не равно null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="action">The action.</param>
        public static void IfNotNull<T>(this T obj, Action<T> action)
        {
            if (obj != null)
                action(obj);
        }

        /// <summary>
        /// Проверка на не null
        /// </summary>
        /// <param name="o">Объект</param>
        /// <returns></returns>
        public static bool IsNotNull(this object o)
        {
            return o != null;
        }

        /// <summary>
        /// Проверка на null
        /// </summary>
        /// <param name="o">Объект</param>
        /// <returns></returns>
        public static bool IsNull(this object o)
        {
            return o == null;
        }

        /// <summary>
        /// Выполнение метода с объектом в случае, если объект не равен null
        /// </summary>
        /// <typeparam name="TSource">Тип входного значения</typeparam>
        /// <typeparam name="TResult">Тип выходного значения</typeparam>
        /// <param name="source">Входное значение</param>
        /// <param name="accessor">Метод</param>
        /// <param name="defaultValue">Значение по умолчанию</param>
        /// <returns></returns>
        public static TResult NavigateTo<TSource, TResult>(this TSource source, Func<TSource, TResult> accessor, TResult defaultValue)
        {
            return ReferenceEquals(source, null) ? defaultValue : accessor(source);
        }

        /// <summary>
        /// Выполнение метода с объектом в случае, если объект не равен null, и возвращение списка значений
        /// </summary>
        /// <typeparam name="TSource">Тип входного значения</typeparam>
        /// <typeparam name="TResult">Тип выходного значения</typeparam>
        /// <param name="source">Входное значение</param>
        /// <param name="accessor">Метод</param>
        /// <param name="defaultValue">Значение по умолчанию</param>
        /// <returns></returns>
        public static IEnumerable<TResult> NavigateToMany<TSource, TResult>(this TSource source, Func<TSource, IEnumerable<TResult>> accessor, IEnumerable<TResult> defaultValue = null)
        {
            return ReferenceEquals(source, null) ? (defaultValue ?? Enumerable.Empty<TResult>()) : accessor(source);
        }

        /// <summary>
        /// Выввод объекта в debug
        /// </summary>
        /// <param name="o">Объект</param>
        public static void PrintToOutput(this object o)
        {
            Debug.WriteLine(o);
        }

        /// <summary>
        /// Вызов деструктора, если объекты не равны
        /// </summary>
        /// <param name="obj1">Оригинальный объект</param>
        /// <param name="obj2">Объект для сравнения</param>
        public static void DisposeIfNotEqualTo(this IDisposable obj1, IDisposable obj2)
        {
            if (!(obj1 == null || obj1.Equals(obj2)))
                obj1.Dispose();
        }
    }
}
