using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace Ak.Framework.Wpf.Binding
{
    /// <summary>
    /// Привязываемые столбцы в DataGrid
    /// </summary>
    public class DataGridColumnsBehavior
    {
        #region Свойства

        /// <summary>
        /// Привязываемые столбцы
        /// </summary>
        public static readonly DependencyProperty BindableColumnsProperty = DependencyProperty.RegisterAttached("BindableColumns", typeof(ObservableCollection<DataGridColumn>), typeof(DataGridColumnsBehavior), new UIPropertyMetadata(null, BindableColumnsPropertyChanged));

        #endregion

        #region Публичные методы

        /// <summary>
        /// Установка привязанных столбцов
        /// </summary>
        /// <param name="element">Элемент</param>
        /// <param name="value">Значение</param>
        public static void SetBindableColumns(DependencyObject element, ObservableCollection<DataGridColumn> value)
        {
            element.SetValue(BindableColumnsProperty, value);
        }

        /// <summary>
        /// Получение привязанные столбцов
        /// </summary>
        /// <param name="element">Элемент</param>
        /// <returns></returns>
        public static ObservableCollection<DataGridColumn> GetBindableColumns(DependencyObject element)
        {
            return (ObservableCollection<DataGridColumn>)element.GetValue(BindableColumnsProperty);
        }

        #endregion

        #region Обработчики событий

        private static void BindableColumnsPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            DataGrid dataGrid = source as DataGrid;
            ObservableCollection<DataGridColumn> columns = e.NewValue as ObservableCollection<DataGridColumn>;
            if (dataGrid != null)
            {
                dataGrid.Columns.Clear();
                if (columns == null)
                    return;

                foreach (DataGridColumn column in columns)
                    dataGrid.Columns.Add(column);

                columns.CollectionChanged += (sender, e2) =>
                {
                    NotifyCollectionChangedEventArgs ne = e2;
                    if (ne.Action == NotifyCollectionChangedAction.Reset)
                    {
                        dataGrid.Columns.Clear();
                        foreach (DataGridColumn column in ne.NewItems)
                            dataGrid.Columns.Add(column);
                    }
                    else if (ne.Action == NotifyCollectionChangedAction.Add)
                    {
                        foreach (DataGridColumn column in ne.NewItems)
                            dataGrid.Columns.Add(column);
                    }
                    else if (ne.Action == NotifyCollectionChangedAction.Move)
                    {
                        dataGrid.Columns.Move(ne.OldStartingIndex, ne.NewStartingIndex);
                    }
                    else if (ne.Action == NotifyCollectionChangedAction.Remove)
                    {
                        foreach (DataGridColumn column in ne.OldItems)
                            dataGrid.Columns.Remove(column);
                    }
                    else if (ne.Action == NotifyCollectionChangedAction.Replace)
                    {
                        dataGrid.Columns[ne.NewStartingIndex] = ne.NewItems[0] as DataGridColumn;
                    }
                };
            }
        }

        #endregion        
    }
}