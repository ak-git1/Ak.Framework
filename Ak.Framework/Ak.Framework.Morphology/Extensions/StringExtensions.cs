using System;
using Ak.Framework.Core.Extensions;

namespace Ak.Framework.Morphology.Extensions
{
    /// <summary>
    /// Расширения для работы со строками
    /// </summary>
    public static class StringExtensions
    {
        #region Публичные методы

        /// <summary>
        /// Возвращает транслитерированную (переведенную из кириллицы в латиницу) строку
        /// </summary>
        /// <param name="str">Входная строка</param>
        /// <returns></returns>
        public static string Transliterate(this string str)
        {
            return Transliteration.Front(str);
        }

        /// <summary>
        /// Преобразование строкового представления числа в текстовое представление с добавлением рублей
        /// </summary>
        /// <param name="str">Входная строка</param>
        /// <returns></returns>
        public static string ToRurText(this string str)
        {
            decimal? dec = str.RemoveWhiteSpaces().ToDecimal(null, true);
            
            if (dec.HasValue)
            {
                bool isNegative = dec < 0;
                if (dec < 0)
                    dec = Math.Abs(dec.Value);

                string[] words = dec.Value.ToRurText().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                words[0] = string.Concat("(", isNegative ? "минус " : string.Empty, words[0]);
                int rubIndex = Array.FindIndex(words, w => w.Contains("руб"));
                if (rubIndex != -1)
                    words[rubIndex - 1] = string.Concat(words[rubIndex - 1], ")");

                int charLastIndex = str.LastIndexOfAny(new []{',', '.'});
                if (charLastIndex > 0)
                    str = str.Substring(0, charLastIndex);

                str = str.RemoveWhiteSpaces();
                str = str.ToCurrencyView();

                str = string.Concat(str, " ", string.Join(" ", words));
            }

            return str;
        }

        #endregion
    }
}