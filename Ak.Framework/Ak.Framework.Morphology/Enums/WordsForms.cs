using System.ComponentModel;

namespace Ak.Framework.Morphology.Enums
{
    /// <summary>
    /// Падежи слов
    /// </summary>
    public enum WordsForms
    {
        /// <summary>
        /// Именительный падеж
        /// </summary>
        [Description("Именительный падеж")]
        Nominative = 1,

        /// <summary>
        /// Родительный падеж
        /// </summary>
        [Description("Родительный падеж")]
        Genitive = 2,

        /// <summary>
        /// Дательный падеж
        /// </summary>
        [Description("Дательный падеж")]
        Dative = 3,

        /// <summary>
        /// Винительный падеж
        /// </summary>
        [Description("Винительный падеж")]
        Accusative = 4,

        /// <summary>
        /// Творительный падеж
        /// </summary>
        [Description("Творительный падеж")]
        Instrumental = 5,

        /// <summary>
        /// Предложный падеж
        /// </summary>
        [Description("Предложный падеж")]
        Prepositional = 6
    }
}
