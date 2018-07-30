using System.Collections.Generic;
using System.IO;
using Ak.Framework.Morphology.Enums;

namespace Ak.Framework.Morphology.Decliners
{
    /// <summary>
    /// Класс для работы с падежами слова
    /// </summary>
    public class WordForms
    {
        #region Свойства

        /// <summary>
        /// Именительный падеж
        /// </summary>
        protected string Nominative { get; set; }

        /// <summary>
        /// Родительный падеж
        /// </summary>
        protected string Genitive { get; set; }

        /// <summary>
        /// Дательный падеж
        /// </summary>
        protected string Dative { get; set; }

        /// <summary>
        /// Винительный падеж
        /// </summary>
        protected string Accusative { get; set; }

        /// <summary>
        /// Творительный падеж
        /// </summary>
        protected string Instrumental { get; set; }

        /// <summary>
        /// Предложный падеж
        /// </summary>
        protected string Prepositional { get; set; }

        #endregion

        #region Конструкторы

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="wordForms">Падежи слова</param>
        public WordForms(IList<string> wordForms)
        {
            if (wordForms.Count > 0)
                Nominative = wordForms[0];

            if (wordForms.Count > 1)
                Genitive = wordForms[1];

            if (wordForms.Count > 2)
                Dative = wordForms[2];

            if (wordForms.Count > 3)
                Accusative = wordForms[3];

            if (wordForms.Count > 4)
                Instrumental = wordForms[4];

            if (wordForms.Count > 5)
                Prepositional = wordForms[5];
        }

        #endregion

        #region Публичные методы

        /// <summary>
        /// Получение слова в заданном падеже
        /// </summary>
        /// <param name="wordsForm">Падеж</param>
        /// <returns></returns>
        public string GetWordForm(WordsForms wordsForm)
        {
            switch (wordsForm)
            {
                case WordsForms.Nominative:
                    return Nominative;

                case WordsForms.Genitive:
                    return Genitive;

                case WordsForms.Dative:
                    return Dative;

                case WordsForms.Accusative:
                    return Accusative;

                case WordsForms.Instrumental:
                    return Instrumental;

                case WordsForms.Prepositional:
                    return Prepositional;

                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Создание словаря с падежами слов
        /// </summary>
        /// <param name="resource">Строка с падежами слов</param>
        /// <returns></returns>
        public static Dictionary<string, WordForms> CreateWordFormsCache(string resource)
        {
            Dictionary<string, WordForms> cashe = new Dictionary<string, WordForms>();
            try
            {
                using (StringReader reader = new StringReader(resource))
                {
                    string line;
                    char[] tab = { '\t' };
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("*"))
                            continue;

                        string[] arr = line.Split(tab);

                        if (arr.Length == 0)
                            continue;

                        if (!cashe.ContainsKey(arr[0]))
                            cashe[arr[0]] = new WordForms(arr);
                    }
                }

            }
            catch
            {
            }

            return cashe;
        }

        #endregion
    }
}
