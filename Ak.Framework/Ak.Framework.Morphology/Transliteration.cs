using System.Collections.Generic;
using Ak.Framework.Morphology.Enums;

namespace Ak.Framework.Morphology
{
    /// <summary>
    /// Класс для транслитерации строк
    /// </summary>
    public static class Transliteration
    {
        #region Переменные и константы

        private static readonly Dictionary<string, string> Gost = new Dictionary<string, string>(); //ГОСТ 16876-71
        private static readonly Dictionary<string, string> Iso = new Dictionary<string, string>(); //ISO 9-95

        #endregion

        #region Конструктор

        static Transliteration()
        {
            Gost.Add("Є", "EH");
            Gost.Add("І", "I");
            Gost.Add("і", "i");
            Gost.Add("№", "N");
            Gost.Add("є", "eh");
            Gost.Add("А", "A");
            Gost.Add("Б", "B");
            Gost.Add("В", "V");
            Gost.Add("Г", "G");
            Gost.Add("Д", "D");
            Gost.Add("Е", "E");
            Gost.Add("Ё", "JO");
            Gost.Add("Ж", "ZH");
            Gost.Add("З", "Z");
            Gost.Add("И", "I");
            Gost.Add("Й", "JJ");
            Gost.Add("К", "K");
            Gost.Add("Л", "L");
            Gost.Add("М", "M");
            Gost.Add("Н", "N");
            Gost.Add("О", "O");
            Gost.Add("П", "P");
            Gost.Add("Р", "R");
            Gost.Add("С", "S");
            Gost.Add("Т", "T");
            Gost.Add("У", "U");
            Gost.Add("Ф", "F");
            Gost.Add("Х", "KH");
            Gost.Add("Ц", "C");
            Gost.Add("Ч", "CH");
            Gost.Add("Ш", "SH");
            Gost.Add("Щ", "SHH");
            Gost.Add("Ъ", "");
            Gost.Add("Ы", "Y");
            Gost.Add("Ь", "");
            Gost.Add("Э", "EH");
            Gost.Add("Ю", "YU");
            Gost.Add("Я", "YA");
            Gost.Add("а", "a");
            Gost.Add("б", "b");
            Gost.Add("в", "v");
            Gost.Add("г", "g");
            Gost.Add("д", "d");
            Gost.Add("е", "e");
            Gost.Add("ё", "jo");
            Gost.Add("ж", "zh");
            Gost.Add("з", "z");
            Gost.Add("и", "i");
            Gost.Add("й", "jj");
            Gost.Add("к", "k");
            Gost.Add("л", "l");
            Gost.Add("м", "m");
            Gost.Add("н", "n");
            Gost.Add("о", "o");
            Gost.Add("п", "p");
            Gost.Add("р", "r");
            Gost.Add("с", "s");
            Gost.Add("т", "t");
            Gost.Add("у", "u");
            Gost.Add("ф", "f");
            Gost.Add("х", "kh");
            Gost.Add("ц", "c");
            Gost.Add("ч", "ch");
            Gost.Add("ш", "sh");
            Gost.Add("щ", "shh");
            Gost.Add("ъ", "");
            Gost.Add("ы", "y");
            Gost.Add("ь", "");
            Gost.Add("э", "eh");
            Gost.Add("ю", "yu");
            Gost.Add("я", "ya");
            Gost.Add("«", "");
            Gost.Add("»", "");
            Gost.Add("—", "-");
            Gost.Add(" ", "_");
            Iso.Add("Є", "YE");
            Iso.Add("І", "I");
            Iso.Add("Ѓ", "G");
            Iso.Add("і", "i");
            Iso.Add("№", "N");
            Iso.Add("є", "ye");
            Iso.Add("ѓ", "g");
            Iso.Add("А", "A");
            Iso.Add("Б", "B");
            Iso.Add("В", "V");
            Iso.Add("Г", "G");
            Iso.Add("Д", "D");
            Iso.Add("Е", "E");
            Iso.Add("Ё", "YO");
            Iso.Add("Ж", "ZH");
            Iso.Add("З", "Z");
            Iso.Add("И", "I");
            Iso.Add("Й", "J");
            Iso.Add("К", "K");
            Iso.Add("Л", "L");
            Iso.Add("М", "M");
            Iso.Add("Н", "N");
            Iso.Add("О", "O");
            Iso.Add("П", "P");
            Iso.Add("Р", "R");
            Iso.Add("С", "S");
            Iso.Add("Т", "T");
            Iso.Add("У", "U");
            Iso.Add("Ф", "F");
            Iso.Add("Х", "X");
            Iso.Add("Ц", "C");
            Iso.Add("Ч", "CH");
            Iso.Add("Ш", "SH");
            Iso.Add("Щ", "SHH");
            Iso.Add("Ъ", "");
            Iso.Add("Ы", "Y");
            Iso.Add("Ь", "");
            Iso.Add("Э", "E");
            Iso.Add("Ю", "YU");
            Iso.Add("Я", "YA");
            Iso.Add("а", "a");
            Iso.Add("б", "b");
            Iso.Add("в", "v");
            Iso.Add("г", "g");
            Iso.Add("д", "d");
            Iso.Add("е", "e");
            Iso.Add("ё", "yo");
            Iso.Add("ж", "zh");
            Iso.Add("з", "z");
            Iso.Add("и", "i");
            Iso.Add("й", "j");
            Iso.Add("к", "k");
            Iso.Add("л", "l");
            Iso.Add("м", "m");
            Iso.Add("н", "n");
            Iso.Add("о", "o");
            Iso.Add("п", "p");
            Iso.Add("р", "r");
            Iso.Add("с", "s");
            Iso.Add("т", "t");
            Iso.Add("у", "u");
            Iso.Add("ф", "f");
            Iso.Add("х", "x");
            Iso.Add("ц", "c");
            Iso.Add("ч", "ch");
            Iso.Add("ш", "sh");
            Iso.Add("щ", "shh");
            Iso.Add("ъ", "");
            Iso.Add("ы", "y");
            Iso.Add("ь", "");
            Iso.Add("э", "e");
            Iso.Add("ю", "yu");
            Iso.Add("я", "ya");
            Iso.Add("«", "");
            Iso.Add("»", "");
            Iso.Add("—", "-");
            Iso.Add(" ", "_");
        }

        #endregion

        #region Публичные методы

        public static string Front(string text)
        {
            return Front(text, TransliterationTypes.Iso);
        }

        public static string Front(string text, TransliterationTypes type)
        {
            string output = text;
            Dictionary<string, string> tdict = GetDictionaryByType(type);
            foreach (KeyValuePair<string, string> key in tdict)
                output = output.Replace(key.Key, key.Value);
            
            return output;
        }

        public static string Back(string text)
        {
            return Back(text, TransliterationTypes.Iso);
        }

        public static string Back(string text, TransliterationTypes type)
        {
            string output = text;
            Dictionary<string, string> tdict = GetDictionaryByType(type);

            foreach (KeyValuePair<string, string> key in tdict)
                output = output.Replace(key.Value, key.Key);

            return output;
        }

        #endregion

        #region Приватные методы

        private static Dictionary<string, string> GetDictionaryByType(TransliterationTypes type)
        {
            Dictionary<string, string> tdict = Iso;
            if (type == TransliterationTypes.Gost) tdict = Gost;
            return tdict;
        }

        #endregion
    }
}
