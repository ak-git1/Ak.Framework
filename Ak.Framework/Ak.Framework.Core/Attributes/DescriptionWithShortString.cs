using System;
using System.ComponentModel;

namespace Ak.Framework.Core.Attributes
{
    /// <summary>
    /// Атрибут описания с расширением, включающим сокращенное описание
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class DescriptionWithShortString : DescriptionAttribute
    {
        #region Свойства

        /// <summary>
        /// Сокращенное описание
        /// </summary>
        public string ShortDescription { get; set; }

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="description">Описание</param>
        /// <param name="shortDescription">Сокращенное описание</param>
        public DescriptionWithShortString(string description, string shortDescription)
        {
            DescriptionValue = description;
            ShortDescription = shortDescription;
        }

        #endregion
    }
}