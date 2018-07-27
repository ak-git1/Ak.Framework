using System.ComponentModel;

namespace Ak.Framework.Core.Enums
{
    /// <summary>
    /// Пол
    /// </summary>
    public enum Genders
    {
        /// <summary>
        /// Не указано
        /// </summary>
        [Description("Не указано")]
        Unknown = 0,

        /// <summary>
        /// Мужской
        /// </summary>
        [Description("Мужской")]
        Male  = 1,

        /// <summary>
        /// Женский
        /// </summary>
        [Description("Женский")]
        Female = 2
    }
}