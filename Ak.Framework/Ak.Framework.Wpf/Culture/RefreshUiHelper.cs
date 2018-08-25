using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup.Primitives;
using System.Windows.Media;

namespace Ak.Framework.Wpf.Culture
{
    /// <summary>
    /// Обновляет зависимости и контролы.
    /// Используется для обновления пользовательского интерфейса при смене языка приложения
    /// </summary>
    public static class RefreshUiHelper
    {
        #region Методы

        /// <summary>
        /// Рекурсивно обновляет зависимости и контролы для заданного элемента и его потомков
        /// </summary>
        /// <param name="element">Элемент</param>
        public static void UpdateRecursive(DependencyObject element)
        {
            if (element == null)
                return;

            int count = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < count; i++)
                UpdateRecursive(VisualTreeHelper.GetChild(element, i));

            UpdateBindingObjects(element);
            UpdateItemsControlIfNeeded(element);
            UpdateComboboxIfNeeded(element);
            UpdateTextBlockRunsIfNeeded(element);
            UpdateTabControlInactiveItems(element);
            UpdatePopupIfNeeded(element);
        }

        /// <summary>
        /// Рекурсивно обновляет контролы типа Combobox для заданного элемента и его потомков
        /// </summary>
        /// <param name="element">Элемент</param>
        private static void UpdateComboboxesRecursive(DependencyObject element)
        {
            if (element == null)
                return;

            int count = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < count; i++)
                UpdateComboboxesRecursive(VisualTreeHelper.GetChild(element, i));

            UpdateItemsControlIfNeeded(element);
            UpdateComboboxIfNeeded(element);
            UpdateTextBlockRunsIfNeeded(element);
        }

        /// <summary>
        /// Обновляет источники данных для заданного элемента
        /// </summary>
        /// <param name="element">Элемент</param>
        private static void UpdateObjectDataSources(DependencyObject element)
        {
            FrameworkElement frameworkElement = element as FrameworkElement;
            if (frameworkElement?.Resources?.Values == null)
                return;

            foreach (object val in frameworkElement.Resources.Values)
                (val as ObjectDataProvider)?.Refresh();
        }

        /// <summary>
        /// Обновляет привязки для заданного элемента
        /// </summary>
        /// <param name="element">Элемент</param>
        private static void UpdateBindingObjects(DependencyObject element)
        {
            UpdateObjectDataSources(element);
            GetProperties(element).ForEach(p =>
            {
                BindingOperations.GetBindingExpression(element, p)?.UpdateTarget();
            });
        }

        /// <summary>
        /// Обновляет неактивные элементы TabControl.
        /// Это вызвано тем, что комбобоксы некоррентно обновляются при изменении языка.
        /// </summary>
        /// <param name="element"></param>
        private static void UpdateTabControlInactiveItems(DependencyObject element)
        {
            TabControl tabControl = element as TabControl;
            if (tabControl?.Items == null || tabControl.Items.Count == 0)
                return;

            foreach (object item in tabControl.Items)
            {
                TabItem tabItem = item as TabItem;

                if (tabItem == null || tabItem.IsSelected || tabItem.Content == null)
                    continue;

                UpdateComboboxesRecursive(tabItem.Content as DependencyObject);
            }
        }

        /// <summary>
        /// Обновляет коллекцию Items для элемента, если он ItemsControl.
        /// </summary>
        /// <param name="element">Элемент</param>
        private static void UpdateItemsControlIfNeeded(DependencyObject element)
        {
            ItemsControl itemsControl = element as ItemsControl;
            if (itemsControl?.Items == null || !itemsControl.HasItems)
                return;

            itemsControl.Items.Refresh();
        }

        /// <summary>
        /// Обновлят визуальное отображение объекта, если он ComboBox.
        /// При изменении языка у ComboBox не меняется отображение выбранного значения.
        /// Чтобы обойти эту проблему процедура меняет выбранное значение на null, а потом на исходное значение.
        /// </summary>
        /// <param name="element">Элемент</param>
        private static void UpdateComboboxIfNeeded(DependencyObject element)
        {
            ComboBox comboBox = element as ComboBox;
            if (comboBox?.SelectedValue == null || comboBox.SelectedItem == null || !comboBox.HasItems || comboBox.Items.Count <= comboBox.SelectedIndex)
                return;

            // Временно отключаю биндинг, если он есть
            System.Windows.Data.Binding selectedItemPropertyBinding = comboBox.GetBindingExpression(Selector.SelectedItemProperty)?.ParentBinding;
            if (selectedItemPropertyBinding != null)
                BindingOperations.ClearBinding(comboBox, Selector.SelectedItemProperty);

            System.Windows.Data.Binding selectedIndexPropertyBinding = comboBox.GetBindingExpression(Selector.SelectedIndexProperty)?.ParentBinding;
            if (selectedIndexPropertyBinding != null)
                BindingOperations.ClearBinding(comboBox, Selector.SelectedIndexProperty);

            System.Windows.Data.Binding selectedValuePathPropertyBinding = comboBox.GetBindingExpression(Selector.SelectedValuePathProperty)?.ParentBinding;
            if (selectedValuePathPropertyBinding != null)
                BindingOperations.ClearBinding(comboBox, Selector.SelectedValuePathProperty);

            System.Windows.Data.Binding selectedValuePropertyBinding = comboBox.GetBindingExpression(Selector.SelectedValueProperty)?.ParentBinding;
            if (selectedValuePropertyBinding != null)
                BindingOperations.ClearBinding(comboBox, Selector.SelectedValueProperty);


            // Меняю значение на null и обратно
            object oldSelectedItem = comboBox.SelectedItem;
            comboBox.SelectedItem = null;
            comboBox.SelectedItem = oldSelectedItem;

            // Возвращаю биндинг назад (если он был)
            if (selectedItemPropertyBinding != null)
                comboBox.SetBinding(Selector.SelectedItemProperty, selectedItemPropertyBinding);

            if (selectedIndexPropertyBinding != null)
                comboBox.SetBinding(Selector.SelectedIndexProperty, selectedIndexPropertyBinding);

            if (selectedValuePathPropertyBinding != null)
                comboBox.SetBinding(Selector.SelectedValuePathProperty, selectedValuePathPropertyBinding);

            if (selectedValuePropertyBinding != null)
                comboBox.SetBinding(Selector.SelectedValueProperty, selectedValuePropertyBinding);
        }

        /// <summary>
        /// Обновляет содержимое элемента, если он Popup
        /// </summary>
        /// <param name="element">Элемент</param>
        private static void UpdatePopupIfNeeded(DependencyObject element)
        {
            Popup popup = element as Popup;
            if (popup == null || popup.IsOpen || popup.Child == null)
                return;

            UpdateComboboxesRecursive(popup.Child);
        }

        /// <summary>
        /// Обновляет элементы типа Run для заданного элемента, если он TextBlock
        /// </summary>
        /// <param name="element">Элемент</param>
        private static void UpdateTextBlockRunsIfNeeded(DependencyObject element)
        {
            TextBlock textBlock = element as TextBlock;
            if (textBlock?.Inlines == null || textBlock.Inlines.Count == 0)
                return;

            foreach (Inline inline in textBlock.Inlines)
                UpdateBindingObjects(inline);
        }

        /// <summary>
        /// Возвращает список привязок для заданного элемента
        /// </summary>
        /// <param name="element">Элемент</param>
        /// <returns>Список привязок для заданного элемента</returns>
        private static List<DependencyProperty> GetProperties(DependencyObject element)
        {
            return MarkupWriter.GetMarkupObjectFor(element)
                .Properties?.Select(p => p.DependencyProperty)
                .Where(dp => dp != null)
                .ToList() ?? new List<DependencyProperty>();
        }

        #endregion
    }
}
