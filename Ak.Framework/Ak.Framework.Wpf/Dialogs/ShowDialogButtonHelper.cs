using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Ak.Framework.Wpf.Dialogs
{
    /// <summary>
    /// Кнопка для работы с диалоговыми оконами
    /// </summary>
    public static class ShowDialogButtonHelper
    {
        #region Переменные

        /// <summary>
        /// Результат выполнения диалога
        /// </summary>
        public static readonly DependencyProperty DialogResultProperty = DependencyProperty.RegisterAttached("DialogResult", typeof(bool?), typeof(ShowDialogButtonHelper), new PropertyMetadata(DialogResultPropertyChangedCallBack));

        #endregion

        #region Публичные методы

        /// <summary>
        /// Получение результата диалогового окна
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <returns></returns>
        public static bool? GetDialogResult(DependencyObject obj)
        {
            return (bool?)obj.GetValue(DialogResultProperty);
        }

        /// <summary>
        /// Установка результата диалогового окна
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <param name="value">Значение</param>
        public static void SetDialogResult(DependencyObject obj, bool? value)
        {
            obj.SetValue(DialogResultProperty, value);
        }

        #endregion

        #region Приватные методы

        /// <summary>
        /// Dialogs the result property changed call back.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void DialogResultPropertyChangedCallBack(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(target))
            {
                Button button = (Button)target;
                button.Click += ButtonClick;
            }
        }

        #endregion

        #region Обработчики событий

        private static void ButtonClick(object sender, RoutedEventArgs e)
        {
            Button dependencyObject = (Button)sender;
            Window window = Window.GetWindow(dependencyObject);
            if (window != null)
                window.DialogResult = GetDialogResult(dependencyObject);
        }

        #endregion      
    }
}
