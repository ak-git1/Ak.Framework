using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace Ak.Framework.Core.Extensions
{
    /// <summary>
    /// Расширения для работы со строками
    /// </summary>
    public static class StringExtensions
    {
        #region Переменные и константы

        /// <summary>
        /// Regex для html комментариев
        /// </summary>
        static readonly Regex HtmlCommentsRegex = new Regex(@"<!--.*?-->", RegexOptions.Singleline);

        /// <summary>
        /// Regex для html тэгов
        /// </summary>
        static readonly Regex HtmlTagsRegex = new Regex(@"<[^>]*>", RegexOptions.Compiled);
        
        /// <summary>
        /// Regex для html input-тэгов
        /// </summary>
        static readonly Regex HtmlInputTagsRegex = new Regex(@"<((input)|(select)|(textarea))[^>]*>", RegexOptions.Compiled);

        /// <summary>
        /// Символы, недопустимые в строке запроса при полнотекстовом поискке
        /// </summary>
        private static readonly string[] SqlFullTextSearchInvalidChars = {",", ".", "!", "?", "\""};

        /// <summary>
        /// Недопустимые символы при инлексировании содержимого сущности
        /// </summary>
        private static readonly string[] XmlSerializationInvalidChars = { "\a", "\n", "\v", "\r", "  " };

        /// <summary>
        /// Коэффициент для рассчёта проверочной цифры ИНН ЮЛ
        /// </summary>
        private static readonly int[] JuridicalInnCheckParams = new int[]
        {
            2,
            4,
            10,
            3,
            5,
            9,
            4,
            6,
            8
        };

        /// <summary>
        /// Коэффициент для рассчёта первой проверочной цифры ИНН ФЛ
        /// </summary>
        private static readonly int[] PhysicalInnCheckParamsFirst = new int[]
        {
            7,
            2,
            4,
            10,
            3,
            5,
            9,
            4,
            6,
            8
        };

        /// <summary>
        /// Коэффициент для рассчёта второй проверочной цифры ИНН ФЛ
        /// </summary>
        private static readonly int[] PhysicalInnCheckParamsSecond = new int[]
        {
            3,
            7,
            2,
            4,
            10,
            3,
            5,
            9,
            4,
            6,
            8
        };

        /// <summary>
        /// The extra spaces regex
        /// </summary>
        private static readonly Regex ExtraSpacesRegex = new Regex(@"\s\s+");

        #endregion

        #region Методы

        /// <summary>
        /// Возвращает строку преобразованную в MD5
        /// </summary>
        /// <param name="str">Строка</param>
        /// <returns></returns>
        public static string ToMd5(this string str)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder sBuilder = new StringBuilder();
            foreach (byte b in data)
                sBuilder.Append(b.ToString("x2"));

            return sBuilder.ToString();
        }

        /// <summary>
        /// Проверка строки на соответствие хэшу MD5
        /// </summary>
        /// <param name="str">Строка</param>
        /// <param name="hash">MD5 хэш</param>
        /// <returns></returns>
        public static bool VerifyMd5(this string str, string hash)
        {
            return StringComparer.OrdinalIgnoreCase.Compare(str.ToMd5(), hash) == 0;
        }

        /// <summary>
        /// Разбирает строку на подстроки внутри разделителей (вложенность не поддерживается)
        /// </summary>
        /// <param name="str">Строка</param>
        /// <param name="firstDelimiter">Первый разделитель</param>
        /// <param name="secondDelimiter">Второй разделитель</param>
        /// <returns></returns>
        public static string[] SplitEx(this string str, string firstDelimiter = "{", string secondDelimiter = "}")
        {
            ArrayList strings = new ArrayList();
            int beginCount = 0;
            int endCount = 0;
            while (str.Length != endCount)
            {
                beginCount = endCount;
                beginCount = str.IndexOf(firstDelimiter, beginCount);

                if (beginCount >= 0)
                    endCount = str.IndexOf(secondDelimiter, beginCount);

                if (endCount < 0 || beginCount < 0)
                    endCount = str.Length;
                else
                {
                    string tmpStr = str.Substring(beginCount + 1, endCount - (beginCount + 1));
                    tmpStr = tmpStr.Trim();
                    if (tmpStr != string.Empty)
                        strings.Add(tmpStr);
                }
            }
            string[] result = new string[strings.Count];
            strings.CopyTo(result);
            return result;
        }

        /// <summary>
        /// Удаляет заданные скобки из строки
        /// </summary>
        /// <param name="paramsString">Входная строка </param>
        /// <param name="brackets">Строка из двух символов - открывающей и закрывающей скобки</param>
        public static string RemoveBrackets(this string paramsString, string brackets = "{}")
        {
            string[] arr = paramsString.Split(brackets.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            return string.Join(string.Empty, arr);
        }

        /// <summary>
        /// Удаление пробелов
        /// </summary>
        /// <param name="str">Строка</param>
        /// <returns></returns>
        public static string RemoveWhiteSpaces(this string str)
        {
            return Regex.Replace(str, @"\s+", string.Empty);
        }

        /// <summary>
        /// Возвращает подстроку номер ParamNum из строки разделенной разделителями Delimiter. Нумерация с подстрок с 1
        /// </summary>
        /// <param name="paramsString">Cтрока с параметрами</param>
        /// <param name="paramNum">Номер параметра (нумерация с 1)</param>
        /// <param name="delimiter">Разделитель</param>
        /// <returns></returns>
        public static string GetParam(this string paramsString, int paramNum, string delimiter = "|")
        {
            try
            {
                string[] strings = paramsString.Split(delimiter[0]);
                return (paramNum <= 0 || strings.Length < paramNum) ? string.Empty : strings[paramNum - 1];
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Добавление параметра в строку
        /// </summary>
        /// <param name="paramsString">Cтрока с параметрами</param>
        /// <param name="param">Параметр</param>
        /// <param name="delimiter">Разделитель</param>
        public static string AddParam(this string paramsString, string param, string delimiter = "|")
        {
            if (!paramsString.Contains(param))
                return (!string.IsNullOrEmpty(paramsString)) ? $"{paramsString}{delimiter}{param}" : param;
            return paramsString;
        }

        /// <summary>
        /// Удаление параметра из строки
        /// </summary>
        /// <param name="paramsString">Cтрока с параметрами</param>
        /// <param name="param">Параметр</param>
        /// <param name="delimiter">Разделитель</param>
        public static string RemoveParam(this string paramsString, string param, string delimiter = "|")
        {
            string tempStr = string.Empty;
            foreach (string p in paramsString.Split(delimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                if (p != param)
                    tempStr = $"{tempStr}{delimiter}{p}";

            return tempStr;
        }

        /// <summary>
        /// Возвращает подстроку по индексу начального символа подстроки
        /// в исходной строке и длине подстроки.
        /// </summary>
        /// <param name="codeString">Cтрока</param>
        /// <param name="startIndex">Индекс начального символа подстроки в исходной строке</param>
        /// <param name="length">Длина подстроки</param>
        /// <returns>Найденная подстрока</returns>
        public static string SubstringEx(this string codeString, int startIndex, int length)
        {
            try
            {
                if (codeString.Length >= startIndex + length)
                    return codeString.Substring(startIndex, length);

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Удаление символов слева
        /// </summary>
        /// <param name="str">Строка</param>
        /// <param name="length">Длина</param>
        /// <returns></returns>
        public static string Left(this string str, int length)
        {
            return str.Substring(0, Math.Min(str.Length, length));
        }

        /// <summary>
        /// Удаление подстроки слева
        /// </summary>
        /// <param name="str">Строка</param>
        /// <param name="subStr">Подстрока</param>
        /// <param name="trimAllInclusions">Полностью удалить подстроку</param>
        public static string TrimLeft(this string str, string subStr, bool trimAllInclusions = false)
        {
            string result = str ?? "";
            do
            {
                result = subStr.NotEmpty() && result.StartsWith(subStr) ? result.Remove(0, subStr.Length) : result;
            } 
            while (trimAllInclusions && subStr.NotEmpty() && result.StartsWith(subStr));

            return result;
        }

        /// <summary>
        /// Удаление символов справа
        /// </summary>
        /// <param name="str">Строка</param>
        /// <param name="length">Длина</param>
        /// <returns></returns>
        public static string Right(this string str, int length)
        {
            return str.Substring(str.Length - Math.Min(str.Length, length), Math.Min(str.Length, length));
        }

        /// <summary>
        /// Не Null и не пустое
        /// </summary>
        /// <param name="str">Строка</param>
        /// <param name="allowWhiteSpaces">Разрешить заполнение пробелами</param>
        /// <param name="allowNewLines">Разрешить символы начала новой строки</param>
        /// <returns></returns>
        public static bool NotEmpty(this string str, bool allowWhiteSpaces = true, bool allowNewLines=true)
        {
            string tempStr = allowNewLines ? str: (str ?? string.Empty).Replace(Environment.NewLine, "");
            return allowWhiteSpaces ? !string.IsNullOrEmpty(tempStr) : !string.IsNullOrWhiteSpace(tempStr);
        }

        /// <summary>
        /// Null или пустое
        /// </summary>
        /// <param name="str">Строка</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// Null, пустое или пробелы
        /// </summary>
        /// <param name="str">Строка</param>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// UrlEncode
        /// </summary>
        /// <param name="str">Строка</param>
        /// <returns></returns>
        public static string UrlEncode(this string str)
        {
            return HttpUtility.UrlEncode(str);
        }

        /// <summary>
        /// UrlDecode
        /// </summary>
        /// <param name="str">Строка</param>
        /// <returns></returns>
        public static string UrlDecode(this string str)
        {
            return HttpUtility.UrlDecode(str);
        }

        /// <summary>
        /// Укоротить длинную строку.
        /// </summary>
        [Obsolete("Используй TruncateString")]
        public static string TrunctateString(this string str, int truncateLength, string endingString = "...")
        {
            return truncateLength > 0 && str.Length > truncateLength
                ? $"{str.Remove(truncateLength)}{endingString}"
                : str;
        }

        /// <summary>
        /// Укоротить длинную строку.
        /// </summary>
        public static string TruncateString(this string str, int truncateLength, string endingString = "...")
        {
            return (truncateLength > 0 && str.Length > truncateLength)
                ? $"{str.Remove(truncateLength)}{endingString}"
                : str;
        }

        /// <summary>
        /// Проверить валидность ИНН ЮЛ
        /// </summary>
        /// <param name="inn">ИНН</param>
        public static bool  ValidateJuridicalInn(this string inn)
        {
            if (string.IsNullOrWhiteSpace(inn))
                return false;
            char[] digits = inn.ToCharArray();
            if (Regex.IsMatch(inn, "(?=^\\d{10}$)(?!^\\d{2}[0]{8}$)"))
                return JuridicalInnCheckParams.Select((t, i) => (int)char.GetNumericValue(digits[i]) * t).Sum() % 11 %
                       10 == (int)char.GetNumericValue(digits[9]);

            return false;
        }

        /// <summary>
        /// Проверить валидность ИНН ФЛ
        /// </summary>
        /// <param name="inn">ИНН</param>
        public static bool ValidatePhysicalInn(this string inn)
        {
            if (string.IsNullOrWhiteSpace(inn))
                return false;
            char[] digits = inn.ToCharArray();
            if (Regex.IsMatch(inn, "^\\d{12}$") &&
                PhysicalInnCheckParamsFirst.Select((t, i) => (int) char.GetNumericValue(digits[i]) * t).Sum() % 11 %
                10 == (int) char.GetNumericValue(digits[10]))
                return PhysicalInnCheckParamsSecond.Select((t, i) => (int) char.GetNumericValue(digits[i]) * t).Sum() %
                       11 % 10 == (int) char.GetNumericValue(digits[11]);

            return false;
        }

        /// <summary>
        /// Проверить валидность ИНН ФЛ или ЮЛ
        /// </summary>
        /// <param name="inn">ИНН</param>
        /// <returns></returns>
        public static bool ValidateInn(this string inn)
        {
            if (!ValidateJuridicalInn(inn))
                return ValidatePhysicalInn(inn);

            return true;
        }

        /// <summary>
        /// Приводит строку "12345678,90" к виду "12 345 678,90"
        /// </summary>
        /// <param name="str">Строка</param>
        /// <param name="delimeter">Разделитель</param>
        /// <param name="defaultAfterComma">Значение после запятой по умолчанию</param>
        /// <returns></returns>
        public static string ToCurrencyView(this string str, string delimeter = "\u00A0", string defaultAfterComma = "")
        {
            string res = string.Empty;
            string afterComma = (str.IndexOf(',') > 0) ? str.Substring(str.IndexOf(',')) : !string.IsNullOrWhiteSpace(defaultAfterComma) ? $",{defaultAfterComma}" : string.Empty;
            if (afterComma.NotEmpty())
                afterComma = afterComma.PadRight(3, '0');

            if (str.IndexOf(',') > 0)
                str = str.Remove(str.IndexOf(','));

            if (str.Length <= 3)
                return $"{str}{afterComma}";

            for (int i = str.Length; i > 0; i -= 3)
                res =
                    $"{str.Substring((i >= 3) ? i - 3 : 0, i - ((i >= 3) ? i - 3 : 0))}{(i == str.Length ? string.Empty : delimeter)}{res}";

            if (afterComma != string.Empty)
                res = $"{res}{afterComma}";

            return res.Trim();
        }

        /// <summary>
        /// Замещает невалидные в XML символы
        /// </summary>
        /// <param name="str">Строка</param>
        /// <returns></returns>
        public static string ToValidXmlString(this string str)
        {
            if (str.NotEmpty())
            {
                str = str.Replace("&", "&amp;").Replace("©", "&copy;").Replace("®", "&reg;").Replace("™", "&trade;").Replace("§", "&sect;");
                return (str.Contains("<") && str.Contains("/>")) ? str : str.Replace(">", "&gt;").Replace("<", "&lt;");
            }

            return string.Empty;
        }

        /// <summary>
        /// Приведение строки к корректному названию файла
        /// </summary>
        /// <param name="str">Строка</param>
        /// <param name="newCharacter">Новый символ, на который производится замена других символов</param>
        /// <returns></returns>
        public static string ToValidFileNameString(this string str, string newCharacter = "")
        {
            foreach (char c in Path.GetInvalidFileNameChars())
                str = str.Replace(c.ToStr(), newCharacter);
            return str;
        }

        /// <summary>
        /// Приведение строки к корректному названию директории
        /// </summary>
        /// <param name="str">Строка</param>
        /// <param name="newCharacter">Новый символ, на который производится замена других символов</param>
        /// <returns></returns>
        public static string ToValidDirectoryNameString(this string str, string newCharacter = "")
        {
            foreach (char c in Path.GetInvalidPathChars())
                str = str.Replace(c.ToStr(), newCharacter);
            return str;
        }

        /// <summary>
        /// Проверка на вхождение строки в исходную строку
        /// </summary>
        /// <param name="source">Строка</param>
        /// <param name="stringToCheck">Строка, вхождение которой проверяется</param>
        /// <param name="comp">Режим проверки</param>
        /// <returns></returns>
        public static bool Contains(this string source, string stringToCheck, StringComparison comp)
        {
            return source?.IndexOf(stringToCheck, comp) >= 0;
        }

        /// <summary>
        /// Проверка на вхождение строки в исходную строку
        /// </summary>
        /// <param name="source">Строка</param>
        /// <param name="stringToCheck">Строка, вхождение которой проверяется</param>
        /// <returns></returns>
        public static bool Contains(this string source, string stringToCheck)
        {
            return source.Contains(stringToCheck, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Удаление html тегов
        /// </summary>
        /// <param name="str">Строка</param>
        /// <returns></returns>
        public static string RemoveHtmlTags(this string str)
        {
            str = str.Replace("<br/>", "\n").Replace("&nbsp;", " ")
                                            .Replace("&ndash; ", " ")
                                            .Replace("&laquo; ", " ")
                                            .Replace("&hellip; ", " ")
                                            .Replace("&raquo; ", " ")
                                            .Replace("&amp;nbsp;", " ")
                                            .Replace("&lt;", "<")
                                            .Replace("&gt;", ">");
            if (str.Contains("<!--"))
                str = HtmlCommentsRegex.Replace(str, string.Empty);
            return HtmlTagsRegex.Replace(str, string.Empty);
        }

        /// <summary>
        /// Замена строковых спец. символов перехода на новую строку на соответсвующие html теги
        /// </summary>
        /// <param name="str">Строка</param>
        /// <returns></returns>
        public static string AddHtmlNewLines(this string str)
        {
            return str?.Replace("\r\n", "<br/>").Replace("\n", "<br/>");
        }

        /// <summary>
        /// Удаление html input-тегов
        /// </summary>
        /// <param name="str">Строка</param>
        /// <returns></returns>
        public static string RemoveHtmlInputTags(this string str)
        {
            if(str.NotEmpty())
                return HtmlInputTagsRegex.Replace(str, string.Empty);
            return str;
        }

        /// <summary>
        /// Удаление пробелов
        /// </summary>
        /// <param name="str">Строка</param>
        /// <returns></returns>
        public static string RemoveSpaces(this string str)
        {
            return str.Replace(" ", string.Empty).Replace("\u00A0", string.Empty);
        }

        /// <summary>
        ///  Добавляет 0 к исходной строке, если она из одного символа. Для корректного отображения часов минут и секунд
        /// </summary>
        /// <param name="str">Строка</param>
        /// <returns></returns>
        public static string ToTimeView(this string str)
        {
            return str.Length == 1 ? $"0{str}" : str;
        }

        /// <summary>
        /// Преобразование строки xml в массив строк
        /// </summary>
        /// <param name="str">Строка</param>
        /// <param name="tagName">Имя тэга</param>
        /// <returns></returns>
        public static string[] ToArrayFromXml(this string str, string tagName)
        {
            List<string> result = new List<string>();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(str);
                XmlNodeList xmlNodeList = doc.SelectNodes($"*/{tagName}");
                if (xmlNodeList != null)
                    foreach (XmlNode node in xmlNodeList)
                        result.Add(node.InnerText);
            }
            catch
            {
            }

            return result.ToArray();
        }

        /// <summary>
        /// Получение числа из строки
        /// </summary>
        /// <param name="str">Строка</param>
        /// <returns></returns>
        public static int? ExtractNumber(this string str)
        {
            return Regex.Match(str, @"\d+").Value.ToInt32(null);
        }

        /// <summary>
        /// Получение кодировки символов
        /// </summary>
        /// <param name="str">Строка, представляющая название или идентификатор кодовой страницы</param>
        /// <param name="сodePage">Идентификатор кодовой страницы, по умолчанию Cyrillic (DOS).</param>
        /// <returns></returns>
        public static Encoding GetEncoding(this string str, int сodePage = 866)
        {
            Encoding encoding = null;

            if (str.NotEmpty())
            {
                int? codePage = str.ToInt32(null);

                try
                {
                    encoding = (codePage.HasValue) ? Encoding.GetEncoding(codePage.Value) : encoding = Encoding.GetEncoding(str);
                }
                catch { }
            }

            return encoding ?? Encoding.GetEncoding(сodePage);
        }

        /// <summary>
        /// Заменяет спец.символы в строке
        /// </summary>
        /// <param name="str">Строка</param>
        /// <param name="replacement">Строка, на которую заменяем спец.символы</param>
        /// <param name="trim">Удалять ли лишние пробельные символы в начале и конце строки</param>
        /// <param name="normalize">Нормализовать ли строку</param>
        /// <param name="specialSymbolsPattern">Шаблон спец.символов для поиска в строке регулярным выражением, по умолчанию "soft hyphen"</param>
        /// <returns></returns>
        public static string TruncateSpecialSymbols(this string str, string replacement = "", bool trim = true, bool normalize = true, string specialSymbolsPattern = "\u00AD")
        {
            if (str != null)
            {
                if (normalize)
                    str = str.Normalize();

                if (str.NotEmpty())
                {
                    str = Regex.Replace(str, specialSymbolsPattern, replacement);
                    if (trim)
                        str = str.Trim();
                }
            }
            return str;
        }

        /// <summary>
        /// Получение форматированной строки, если не 
        /// </summary>
        /// <param name="str">Строка</param>
        /// <param name="format">Формат</param>
        /// <returns></returns>
        public static string FormatIfNotEmpty(this string str, string format)
        {
            return str.NotEmpty() ? string.Format(format, str) : string.Empty;
        }

        /// <summary>
        /// Сравнивает строки, содержащие коды
        /// </summary>
        /// <param name="str">Строка</param>
        /// <param name="otherString">Другая строка</param>
        /// <returns></returns>
        public static int CompareParts(this string str, string otherString)
        {
            string[] parts = str.Split('.');
            string[] otherParts = otherString.Split('.');

            int index = 0;
            while (true)
            {
                bool hasValue = parts.Length > index;
                bool otherHasValue = otherParts.Length > index;
                if (hasValue && !otherHasValue)
                    return 1;
                if (!hasValue && otherHasValue)
                    return -1;
                if (!hasValue && !otherHasValue)
                    return 0;
                string value = parts[index];
                string otherValue = otherParts[index];
                if (string.Compare(value, otherValue, StringComparison.CurrentCulture) != 0)
                {
                    if (value.NotEmpty() && !otherValue.NotEmpty())
                        return 1;
                    if (!value.NotEmpty() && otherValue.NotEmpty())
                        return -1;
                    bool isNumber = Regex.IsMatch(value, "^[0-9]+[0-9 ]*$");
                    bool otherIsNumber = Regex.IsMatch(otherValue, "^[0-9]+[0-9 ]*$");

                    if (!isNumber && !otherIsNumber)
                    {
                        Match match = Regex.Match(value, "^([^0-9]+)([0-9]+[0-9 ]*$)$");
                        Match otherMatch = Regex.Match(otherValue, "^([^0-9]+)([0-9]+[0-9 ]*$)$");

                        if (match.Success && otherMatch.Success)
                            if (match.Groups[1].Value != otherMatch.Groups[1].Value)
                                return string.Compare(match.Groups[1].Value, otherMatch.Groups[1].Value, StringComparison.CurrentCulture);
                            else
                            {
                                isNumber = otherIsNumber = true;
                                value = match.Groups[2].Value;
                                otherValue = otherMatch.Groups[2].Value;
                            }
                    }
                    if (!(isNumber && otherIsNumber))
                        return string.Compare(value, otherValue, StringComparison.CurrentCulture);

                    double numberValue = double.Parse(value.Replace(" ", ""), CultureInfo.InvariantCulture);
                    double otherNumberValue = double.Parse(otherValue.Replace(" ", ""), CultureInfo.InvariantCulture);
                    if (numberValue > otherNumberValue)
                        return 1;
                    if (numberValue < otherNumberValue)
                        return -1;
                }
                index++;
            }
        }

        /// <summary>
        /// Отступ слева, заполняемый пробелами
        /// </summary>
        /// <param name="str">Строка</param>
        /// <param name="length">Количество пробелов</param>
        /// <param name="space">Символ отступа</param>
        /// <returns></returns>
        public static string PaddingLeft(this string str, int length, string space = "&nbsp;")
        {
            string s = "".PadLeft(length);
            return string.Concat(s.Replace(" ", space), str);
        }

        /// <summary>
        /// Удаление символов, недопустимых при формировании XML
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveIndexingInvalidChars(this string str)
        {
            StringBuilder filteredStr =  new StringBuilder(str.ToValidXmlString());
            foreach (string serializationInvalidChar in XmlSerializationInvalidChars)
            {
                filteredStr.Replace(serializationInvalidChar, "");
            }
            return filteredStr.ToStr();
        }

        /// <summary>
        /// Формирование строки запроса к сервису полнотекстового поиска
        /// </summary>
        /// <param name="str">Строка запроса</param>
        /// <returns>Отфильтрованная строка запроса</returns>
        public static string ToFullTextSearchValidString(this string str)
        {
            StringBuilder tmp = new StringBuilder(str);
            foreach (string invalidChar in SqlFullTextSearchInvalidChars)
            {
                tmp.Replace(invalidChar, "");
            }
            List<string> mas = tmp.ToStr().Split(' ').ToListEx();
            return string.Join(", ", mas.WhereEx(s => !string.IsNullOrWhiteSpace(s)).Select(s => $"\"{s}\"").ToListEx());
        }

        /// <summary>
        /// Получить строку для сравнения (только алфавитные символы, приведённые в нижний регистр и цифры)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetToCompareString(this string str)
        {
            return Regex.Replace(str ?? string.Empty, @"\W", string.Empty).ToLower().Replace("ё", "е");
        }

        /// <summary>
        /// Мягкое сравнение строк (только алфавитные символы, приведённые в нижний регистр и цифры)
        /// </summary>
        /// <param name="str1">Строка 1</param>
        /// <param name="str2">Строка 2</param>
        /// <returns></returns>
        public static bool SoftStringEqual(this string str1, string str2)
        {
            return str1.GetToCompareString() == str2.GetToCompareString();
        }

        /// <summary>
        /// Удаление пунктуации
        /// </summary>
        /// <param name="str">Строка</param>
        /// <returns></returns>
        public static string RemovePunctuation(this string str)
        {
            return new string(str.Where(c => !char.IsPunctuation(c)).ToArray());
        }

        /// <summary>
        /// Удаление разделителя строки
        /// </summary>
        /// <param name="str">Строка</param>
        /// <param name="replacementString">Строка замены</param>
        /// <returns></returns>
        public static string RemoveLineBreaks(this string str, string replacementString = " ")
        {
            return str.Replace(Environment.NewLine, replacementString);
        }

        /// <summary>
        /// Удаление всех символов кроме чисел
        /// </summary>
        /// <param name="str">Строка</param>
        /// <returns></returns>
        public static string RemoveAllSymbolsExceptNumbers(this string str)
        {
            return new string(str.Where(char.IsNumber).ToArray());
        }

        /// <summary>
        /// Удаление всех символов кроме букв
        /// </summary>
        /// <param name="str">Строка</param>
        /// <returns></returns>
        public static string RemoveAllSymbolsExceptLetters(this string str)
        {
            return new string(str.Where(char.IsLetter).ToArray());
        }

        /// <summary>
        /// Удаление задвоенных пробелов
        /// </summary>
        /// <param name="str">Строка</param>
        /// <returns></returns>
        public static string RemoveExtraSpaces(this string str)
        {           
            return ExtraSpacesRegex.Replace(str, " ");
        }

        #endregion
    }
}