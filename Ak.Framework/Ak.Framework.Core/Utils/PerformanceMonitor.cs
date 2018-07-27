using System;
using System.Diagnostics;

namespace Ak.Framework.Core.Utils
{
    /// <summary>
    /// Монитор производительности
    /// </summary>
    public class PerformanceMonitor : IDisposable
    {
        #region Переменные

        /// <summary>
        /// Сообщение
        /// </summary>
        private readonly string _message;

        /// <summary>
        /// Действие
        /// </summary>
        private readonly Action<string> _messageAction;

        /// <summary>
        /// Таймер
        /// </summary>
        private readonly Stopwatch _stopwatch = new Stopwatch();

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="messageAction">Действие</param>
        public PerformanceMonitor(string message, Action<string> messageAction = null)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            _message = message;

            if (messageAction == null)
                _messageAction = msg => Debug.WriteLine(msg);
            else
                _messageAction = messageAction;

            _messageAction($"{_message}...");
            _stopwatch.Start();
        }

        #endregion

        #region Деструктор

        /// <summary>
        /// Деструктор
        /// </summary>
        public void Dispose()
        {
            _stopwatch.Stop();
            _messageAction($"{_message} done. Time: {_stopwatch.ElapsedMilliseconds} ms");
        }

        #endregion
    }
}
