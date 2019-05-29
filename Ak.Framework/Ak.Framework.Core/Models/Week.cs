using System;
using Ak.Framework.Core.Extensions;

namespace Ak.Framework.Core.Models
{
    /// <summary>
    /// Неделя
    /// </summary>
    public class Week
    {
        #region Свойства

        /// <summary>
        /// Дата начала недели
        /// </summary>
        public DateTime WeekStartDate { get; set; }

        /// <summary>
        /// Дата окончания недели
        /// </summary>
        public DateTime WeekEndDate { get; set; }

        /// <summary>
        /// Номер недели
        /// </summary>
        public int WeekNumber => WeekEndDate.GetIso8601WeekOfYear();

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="weekStartDate">Дата начала недели</param>
        /// <param name="weekEndDate">Дата окончания недели</param>
        public Week(DateTime weekStartDate, DateTime weekEndDate)
        {
            WeekStartDate = weekStartDate;
            WeekEndDate = weekEndDate;
        }

        #endregion
    }
}
