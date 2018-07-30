using System;
using System.Collections.Generic;
using Ak.Framework.Morphology.Enums;

namespace Ak.Framework.Morphology.Decliners
{
    /// <summary>
    /// Класс для склонения должностей 
    /// </summary>
    public class PositionsDecliner
    {
        #region Переменные и константы

        /// <summary>
        /// Словарь склонений должностей
        /// </summary>
        private static readonly Dictionary<string, WordForms> Cashe;

        #endregion

        #region Свойства

        /// <summary>
        /// Словарь склонений родительного падежа должностей
        /// </summary>
        private static Dictionary<string, WordForms> PositionsDecliners
        {
            get
            {
                if (Cashe == null)
                    throw new NullReferenceException("Не инициализирована таблица PositionsDecliner");
                return Cashe;
            }
        }

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        static PositionsDecliner()
        {
            Cashe = WordForms.CreateWordFormsCache(Properties.Resources.positions_forms); 
        }

        #endregion

        #region Публичные методы

        /// <summary>
        /// Склонение
        /// </summary>
        /// <param name="position">Должность</param>
        /// <param name="wordsForm">Падеж</param>
        /// <param name="isToLowerCase">Приводить к нижнему регистру</param>
        /// <returns></returns>
        public static string Decline(string position, WordsForms wordsForm = WordsForms.Nominative, bool isToLowerCase = true)
        {
            string[] words = position.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                string wordLowerCase = isToLowerCase ? words[i].ToLower() : words[i];
                words[i] = PositionsDecliners.ContainsKey(wordLowerCase)
                    ? PositionsDecliners[wordLowerCase].GetWordForm(wordsForm)
                    : wordLowerCase;
            }
            return string.Join(" ", words);
        }

        #endregion
    }
}