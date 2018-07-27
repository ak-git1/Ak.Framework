using System;

namespace Ak.Framework.Core.Events
{
    /// <summary>
    /// Аргументы события с информацией об исключении
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public sealed class ExceptionEventArgs : EventArgs
    {
        #region Свойства

        /// <summary>
        /// Исключение
        /// </summary>
        public Exception Exception { get; }

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="exception">Исключение</param>
        public ExceptionEventArgs(Exception exception)
        {
            Exception = exception;
        }

        #endregion
    }
}
