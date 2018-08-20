using System.Windows;

namespace Ak.Framework.Wpf.Dialogs.Interfaces
{
    /// <summary>
    /// Интерфейс класса для отображения диалоговых окон
    /// </summary>
    public interface IShowDialogAgent
    {
        /// <summary>
        /// Отображение диалога
        /// </summary>
        /// <typeparam name="TDialogWindowType">Тип диалогового окна</typeparam>
        /// <param name="dialogViewModel">ViewModel диалогового окна</param>
        /// <returns></returns>
        bool? ShowDialog<TDialogWindowType>(object dialogViewModel) where TDialogWindowType : Window, new();

        /// <summary>
        /// Отображение диалога
        /// </summary>
        /// <typeparam name="TDialogWindowType">Тип диалогового окна</typeparam>
        /// <param name="dialogViewModel">ViewModel диалогового окна</param>
        /// <param name="dialog">Диалоговое окно</param>
        /// <returns></returns>
        bool? ShowDialog<TDialogWindowType>(object dialogViewModel, out TDialogWindowType dialog) where TDialogWindowType : Window, new();

        /// <summary>
        /// Отображение сообщения
        /// </summary>
        /// <param name="messageBoxText">Текст сообщения</param>
        /// <param name="caption">Заголовок</param>
        /// <param name="button">Кнопк</param>
        /// <param name="icon">Иконка</param>
        /// <returns></returns>
        MessageBoxResult ShowMessageBox(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon);

        /// <summary>
        /// Активное (текущее окно)
        /// </summary>
        Window ActiveWindow { get; }
    }
}
