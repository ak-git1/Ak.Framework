using System;
using System.Windows.Input;

namespace Ak.Framework.Wpf.Interfaces
{
    /// <summary>
    /// Интерфейс команды с нотификацией
    /// </summary>
    public interface INotifyCommand : ICommand
    {
        /// <summary>
        /// Событие исполнения
        /// </summary>
        event EventHandler Executed;

        /// <summary>
        /// Нотификация о том, что изменилась проверка возможности имполнения
        /// </summary>
        void NotifyCanExecuteChanged();
    }
}
