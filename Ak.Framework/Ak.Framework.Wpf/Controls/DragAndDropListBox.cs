using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Ak.Framework.Wpf.Controls
{
    /// <summary>
    /// ListBox с функциональностью Drag and drop
    /// </summary>
    /// <typeparam name="T">Тип</typeparam>
    /// <seealso cref="System.Windows.Controls.ListBox" />
    public class DragAndDropListBox<T> : ListBox where T : class
    {
        #region Переменные и константы

        /// <summary>
        /// Точка для Drag and drop
        /// </summary>
        private Point _dragStartPoint;

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        public DragAndDropListBox()
        {
            PreviewMouseMove += ListBox_PreviewMouseMove;

            Style style = new Style(typeof(ListBoxItem));
            style.Setters.Add(new Setter(AllowDropProperty, true));
            style.Setters.Add(new EventSetter(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(ListBoxItem_PreviewMouseLeftButtonDown)));
            style.Setters.Add(new EventSetter(DropEvent, new DragEventHandler(ListBoxItem_Drop)));
            ItemContainerStyle = style;
        }

        #endregion

        #region Приватные методы

        /// <summary>
        /// Перемещение
        /// </summary>
        /// <param name="source">Начальный элемент</param>
        /// <param name="sourceIndex">Индекс начального элемента</param>
        /// <param name="targetIndex">Конечный элемент</param>
        private void Move(T source, int sourceIndex, int targetIndex)
        {
            if (sourceIndex < targetIndex)
            {
                if (DataContext is IList<T> items)
                {
                    items.Insert(targetIndex + 1, source);
                    items.RemoveAt(sourceIndex);
                }
            }
            else
            {
                if (DataContext is IList<T> items)
                {
                    int removeIndex = sourceIndex + 1;
                    if (items.Count + 1 > removeIndex)
                    {
                        items.Insert(targetIndex, source);
                        items.RemoveAt(removeIndex);
                    }
                }
            }
        }

        /// <summary>
        /// Нахождение родителя в VisualTree
        /// </summary>
        /// <typeparam name="P">Тип</typeparam>
        /// <param name="child">Дочерний элемент</param>
        /// <returns></returns>
        private P FindVisualParent<P>(DependencyObject child) where P : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null)
                return null;

            if (parentObject is P parent)
                return parent;

            return FindVisualParent<P>(parentObject);
        }

        #endregion

        #region Обработчики событий

        private void ListBox_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(null);
            Vector diff = _dragStartPoint - point;
            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                ListBoxItem lbi = FindVisualParent<ListBoxItem>((DependencyObject)e.OriginalSource);
                if (lbi != null)
                    DragDrop.DoDragDrop(lbi, lbi.DataContext, DragDropEffects.Move);
            }
        }

        private void ListBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _dragStartPoint = e.GetPosition(null);
        }

        private void ListBoxItem_Drop(object sender, DragEventArgs e)
        {
            if (sender is ListBoxItem)
            {
                T source = e.Data.GetData(typeof(T)) as T;
                T target = ((ListBoxItem)(sender)).DataContext as T;

                int sourceIndex = this.Items.IndexOf(source);
                int targetIndex = this.Items.IndexOf(target);

                Move(source, sourceIndex, targetIndex);
            }
        }

        #endregion
    }
}
