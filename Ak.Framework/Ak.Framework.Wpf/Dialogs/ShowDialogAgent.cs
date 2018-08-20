using System.Windows;
using System.Windows.Threading;
using Ak.Framework.Wpf.Dialogs.Interfaces;

namespace Ak.Framework.Wpf.Dialogs
{
    /// <summary>
    /// Класс для отображения диалоговых окон
    /// </summary>
    /// <seealso cref="IShowDialogAgent" />
    public class ShowDialogAgent : IShowDialogAgent
    {
        #region Переменные

        /// <summary>
        /// Активное (текущее) окно
        /// </summary>
        private Window _activeWindow;

        /// <summary>
        /// Диспетчер
        /// </summary>
        private static readonly Dispatcher Dispatcher = Application.Current.Dispatcher;

        #endregion

        #region

        /// <summary>
        /// Активное (текущее окно)
        /// </summary>
        public Window ActiveWindow
        {
            get
            {
                if ((_activeWindow == null) || ((_activeWindow.Owner == null) && !_activeWindow.Equals(Application.Current.MainWindow)))
                {
                    _activeWindow = Application.Current.MainWindow;
                }
                return _activeWindow;
            }
            private set
            {
                _activeWindow = value;
            }
        }

        /// <summary>
        /// Синглтон класса для отображения диалоговых окон
        /// </summary>
        public static IShowDialogAgent Instance { get; } = new ShowDialogAgent();

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        private ShowDialogAgent()
        {
        }

        #endregion

        #region Публичные методы

        /// <summary>
        /// Отображение диалога
        /// </summary>
        /// <typeparam name="TDialogWindowType">Тип диалогового окна</typeparam>
        /// <param name="dialogViewModel">ViewModel диалогового окна</param>
        /// <returns></returns>
        public bool? ShowDialog<TDialogWindowType>(object dialogViewModel) where TDialogWindowType : Window, new()
        {
            return Dispatcher.Invoke(() => ShowDialogImpl<TDialogWindowType>(dialogViewModel));
        }

        /// <summary>
        /// Отображение диалога
        /// </summary>
        /// <typeparam name="TDialogWindowType">Тип диалогового окна</typeparam>
        /// <param name="dialogViewModel">ViewModel диалогового окна</param>
        /// <param name="dialog">Диалоговое окно</param>
        /// <returns></returns>
        public bool? ShowDialog<TDialogWindowType>(object dialogViewModel, out TDialogWindowType dialog) where TDialogWindowType : Window, new()
        {
            bool? result = null;
            dialog = Dispatcher.Invoke(() => ShowDialogImpl<TDialogWindowType>(dialogViewModel, out result));
            return result;
        }

        /// <summary>
        /// Отображение сообщения
        /// </summary>
        /// <param name="messageBoxText">Текст сообщения</param>
        /// <param name="caption">Заголовок</param>
        /// <param name="button">Кнопк</param>
        /// <param name="icon">Иконка</param>
        /// <returns></returns>
        public MessageBoxResult ShowMessageBox(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            if (Application.Current == null)
            {
                return MessageBoxResult.None;
            }
            MessageBoxResult result = MessageBoxResult.None;
            Dispatcher.Invoke(delegate {
                result = ActiveWindow == null ? MessageBox.Show(messageBoxText, caption, button, icon) : MessageBox.Show(ActiveWindow, messageBoxText, caption, button, icon);
            });
            return result;
        }

        #endregion

        #region Приватные методы

        /// <summary>
        /// Обработка отображения диалогового окна
        /// </summary>
        /// <typeparam name="TDialogWindowType">Тип</typeparam>
        /// <param name="dialogViewModel">ViewModel диалогового окна</param>
        /// <returns></returns>
        private bool? ShowDialogImpl<TDialogWindowType>(object dialogViewModel) where TDialogWindowType : Window, new()
        {
            bool? nullable;
            Window activeWindow = ActiveWindow;
            try
            {
                Window window2 = new TDialogWindowType();
                window2.Owner = activeWindow;
                window2.DataContext = dialogViewModel;
                ActiveWindow = window2;
                nullable = window2.ShowDialog();
            }
            finally
            {
                ActiveWindow = activeWindow;
            }
            return nullable;
        }

        /// <summary>
        /// Обработка отображения диалогового окна
        /// </summary>
        /// <typeparam name="TDialogWindowType">Тип</typeparam>
        /// <param name="dialogViewModel">ViewModel диалогового окна</param>
        /// <param name="result">Диалоговое окно</param>
        /// <returns></returns>
        private TDialogWindowType ShowDialogImpl<TDialogWindowType>(object dialogViewModel, out bool? result) where TDialogWindowType : Window, new()
        {
            TDialogWindowType local2;
            Window activeWindow = ActiveWindow;
            try
            {
                TDialogWindowType local = new TDialogWindowType
                {
                    Owner = activeWindow,
                    DataContext = dialogViewModel
                };
                ActiveWindow = local;
                result = local.ShowDialog();
                local2 = local;
            }
            finally
            {
                ActiveWindow = activeWindow;
            }
            return local2;
        }

        #endregion              
    }
}
