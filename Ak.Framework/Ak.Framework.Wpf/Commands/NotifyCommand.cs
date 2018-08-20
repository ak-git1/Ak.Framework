using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Ak.Framework.Core.Events;
using Ak.Framework.Core.Helpers;
using Ak.Framework.Wpf.Commands.Interfaces;
using NLog;

namespace Ak.Framework.Wpf.Commands
{
    /// <summary>
    /// Команда нотификации
    /// </summary>
    /// <seealso cref="INotifyCommand" />
    public class NotifyCommand : INotifyCommand
    {
        #region Переменные

        /// <summary>
        /// Функция проверка возможности исполнения
        /// </summary>
        private readonly Func<object, bool> _canExecute;

        /// <summary>
        /// Действие при исключении
        /// </summary>
        private readonly Action<Exception> _exceptionAction;

        /// <summary>
        /// Основной обработчик действия
        /// </summary>
        private readonly Action<object> _execute;

        /// <summary>
        /// Логгер
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Исключение при работе команды
        /// </summary>
        public static EventHandler<ExceptionEventArgs> CommandException;

        /// <summary>
        /// Событие, если изменился признак возможности исполнения
        /// </summary>
        private event EventHandler _canExecuteChanged;

        #endregion

        #region События

        /// <summary>
        /// Событие изменения проверки возможности исполнения
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                _canExecuteChanged += value;
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                _canExecuteChanged -= value;
                CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        /// Событие исполнения
        /// </summary>
        public event EventHandler Executed;

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="execute">Действие</param>
        public NotifyCommand(Action<object> execute) : this(execute, null, DefaultExceptionHandler)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="execute">Действие</param>
        /// <param name="canExecute">Проверка возможности выполнения действия</param>
        public NotifyCommand(Action<object> execute, Func<object, bool> canExecute) : this(execute, canExecute, DefaultExceptionHandler)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="execute">Действие</param>
        /// <param name="canExecute">Проверка возможности выполнения действия</param>
        /// <param name="exceptionAction">Действие при исключении</param>
        public NotifyCommand(Action<object> execute, Func<object, bool> canExecute, Action<Exception> exceptionAction)
        {
            ThrowHelper.CheckNotNull(() => execute);
            ThrowHelper.CheckNotNull(() => exceptionAction);
            _execute = execute;
            _canExecute = canExecute;
            _exceptionAction = exceptionAction;
        }

        #endregion

        #region Публичные методы

        /// <summary>
        /// Проверка возможности исполнения
        /// </summary>
        /// <param name="parameter">Параметр</param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            try
            {
                return _canExecute == null || _canExecute(parameter);
            }
            catch (Exception exception)
            {
                _exceptionAction(exception);
                return false;
            }
        }

        /// <summary>
        /// Исполнение
        /// </summary>
        /// <param name="parameter">Параметр</param>
        public void Execute(object parameter)
        {
            try
            {
                _execute(parameter);
                Executed?.Invoke(this, new EventArgs());
            }
            catch (Exception exception)
            {
                _exceptionAction(exception);
            }
        }

        /// <summary>
        /// Нотификация всег слушателей о том, что изменилась проверка возможности имполнения
        /// </summary>
        public static void NotifyCanExecuteChangedForAll()
        {
            if (Application.Current.Dispatcher.CheckAccess())
                CommandManager.InvalidateRequerySuggested();
            else
                Application.Current.Dispatcher.InvokeAsync(CommandManager.InvalidateRequerySuggested, DispatcherPriority.Normal);
        }

        /// <summary>
        /// Нотификация о том, что изменилась проверка возможности имполнения
        /// </summary>
        public void NotifyCanExecuteChanged()
        {
            Action callback = null;
            if (_canExecuteChanged != null)
            {
                if (Application.Current.Dispatcher.CheckAccess())
                {
                    _canExecuteChanged(this, EventArgs.Empty);
                }
                else
                {
                    if (callback == null)
                        callback = () => _canExecuteChanged(this, EventArgs.Empty);

                    Application.Current.Dispatcher.InvokeAsync(callback, DispatcherPriority.Normal);
                }
            }
        }

        #endregion

        #region Приватные методы

        /// <summary>
        /// Обработчик исключения по умолчанию
        /// </summary>
        /// <param name="ex">Исключение</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void DefaultExceptionHandler(Exception ex)
        {
            Action callback = null;
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                if (callback == null)
                    callback = () => DefaultExceptionHandler(ex);

                Application.Current.Dispatcher.InvokeAsync(callback, DispatcherPriority.Normal);
            }
            else
            {
                Exception innerException = ex;
                while (innerException.InnerException != null)
                    innerException = innerException.InnerException;

                Logger.Error(innerException, $"Exception in command handler: {innerException.Message}.");
                CommandException.Raise(null, new ExceptionEventArgs(ex));
            }
        }

        #endregion
    }
}