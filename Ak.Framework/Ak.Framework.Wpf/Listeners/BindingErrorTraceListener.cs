using System;
using System.Diagnostics;
using System.Text;
using System.Windows;

namespace Ak.Framework.Wpf.Listeners
{
    /// <summary>
    /// Листенер для диагностики проблем с биндингом
    /// </summary>
    /// <seealso cref="System.Diagnostics.DefaultTraceListener" />
    public class BindingErrorTraceListener : DefaultTraceListener
    {
        #region Переменные

        /// <summary>
        /// Листенер
        /// </summary>
        private static BindingErrorTraceListener _listener;

        /// <summary>
        /// Сообщение
        /// </summary>
        private readonly StringBuilder _message = new StringBuilder();

        /// <summary>
        /// Отображать сообщение об ошибке в диалоговом окне
        /// </summary>
        private static bool _showWindowOnError;

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        private BindingErrorTraceListener()
        {
        }

        #endregion

        #region Публичные методы

        /// <summary>
        /// Запуск отслеживания
        /// </summary>
        /// <param name="showWindowOnError">Отображать сообщение об ошибке в диалоговом окне</param>
        public static void SetTrace(bool showWindowOnError = true)
        {
            _showWindowOnError = showWindowOnError;
            SetTrace(SourceLevels.Error, TraceOptions.None);
        }

        /// <summary>
        /// Запуск отслеживания
        /// </summary>
        /// <param name="level">Уровень сообщений при отслеживании</param>
        /// <param name="options">Опции</param>
        public static void SetTrace(SourceLevels level, TraceOptions options)
        {
            if (_listener == null)
            {
                _listener = new BindingErrorTraceListener();
                PresentationTraceSources.DataBindingSource.Listeners.Add(_listener);
            }

            _listener.TraceOutputOptions = options;
            PresentationTraceSources.DataBindingSource.Switch.Level = level;
        }

        /// <summary>
        /// Завершение отслеживания
        /// </summary>
        public static void CloseTrace()
        {
            if (_listener == null)
                return;

            _listener.Flush();
            _listener.Close();
            PresentationTraceSources.DataBindingSource.Listeners.Remove(_listener);
            _listener = null;
        }

        /// <summary>
        /// Writes the output to the OutputDebugString function and to the <see cref="M:System.Diagnostics.Debugger.Log(System.Int32,System.String,System.String)" /> method.
        /// </summary>
        /// <param name="message">The message to write to OutputDebugString and <see cref="M:System.Diagnostics.Debugger.Log(System.Int32,System.String,System.String)" />.</param>
        public override void Write(string message)
        {
            _message.Append(message);
            if (!_showWindowOnError)
                Console.WriteLine(message);
        }

        /// <summary>
        /// Writes the output to the OutputDebugString function and to the <see cref="M:System.Diagnostics.Debugger.Log(System.Int32,System.String,System.String)" /> method, followed by a carriage return and line feed (\r\n).
        /// </summary>
        /// <param name="message">The message to write to OutputDebugString and <see cref="M:System.Diagnostics.Debugger.Log(System.Int32,System.String,System.String)" />.</param>
        public override void WriteLine(string message)
        {
            _message.Append(message);
            string str = _message.ToString();
            _message.Length = 0;

            if (_showWindowOnError)
                MessageBox.Show(str, "Binding Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                Console.WriteLine(str);
        }

        #endregion        
    }
}