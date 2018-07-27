using System.ComponentModel;

namespace Ak.Framework.Core.Enums
{
    /// <summary>
    /// Типы временных интервалов
    /// </summary>
    public enum TimeIntervalTypes
    {
        /// <summary>
        /// Миллисекунда
        /// </summary>
        [Description("Миллисекунда")]
        Millisecond = 1,

        /// <summary>
        /// Секунда
        /// </summary>
        [Description("Секунда")]
        Second = 2,

        /// <summary>
        /// Минута
        /// </summary>
        [Description("Минута")]
        Minute = 3,

        /// <summary>
        /// Час
        /// </summary>
        [Description("Час")]
        Hour = 4,

        /// <summary>
        /// День
        /// </summary>
        [Description("День")]
        Day = 5,

        /// <summary>
        /// Неделя
        /// </summary>
        [Description("Неделя")]
        Week = 6,

        /// <summary>
        /// Месяц
        /// </summary>
        [Description("Месяц")]
        Month = 7,

        /// <summary>
        /// Квартал
        /// </summary>
        [Description("Квартал")]
        Quarter = 8,

        /// <summary>
        /// Год
        /// </summary>
        [Description("Год")]
        Year = 9
    }
}