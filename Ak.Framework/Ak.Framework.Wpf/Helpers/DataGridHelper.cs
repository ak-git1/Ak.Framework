using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Ak.Framework.Wpf.Helpers
{
    /// <summary>
    /// Класс для упрощения работы с DataGrid
    /// </summary>
    public static class DataGridHelper
    {
        /// <summary>
        /// Получение DataGridCell из DataGridCellInfo
        /// </summary>
        /// <param name="dataGridCellInfo">DataGridCellInfo</param>
        /// <returns></returns>
        public static DataGridCell GetCell(DataGridCellInfo dataGridCellInfo)
        {
            if (!dataGridCellInfo.IsValid)
                return null;

            FrameworkElement cellContent = dataGridCellInfo.Column.GetCellContent(dataGridCellInfo.Item);
            return (DataGridCell)cellContent?.Parent;
        }

        /// <summary>
        /// Получение индекса строки для ячейки
        /// </summary>
        /// <param name="dataGridCell">Ячейка</param>
        /// <returns></returns>
        public static int GetRowIndex(DataGridCell dataGridCell)
        {
            PropertyInfo rowDataItemProperty = dataGridCell.GetType().GetProperty("RowDataItem", BindingFlags.Instance | BindingFlags.NonPublic);
            DataGrid dataGrid = GetDataGridFromChild(dataGridCell);
            return dataGrid.Items.IndexOf(rowDataItemProperty.GetValue(dataGridCell, null));
        }

        /// <summary>
        /// Получение DataGrid из дочернего элемента
        /// </summary>
        /// <param name="dataGridPart">Дочерний элемент</param>
        /// <returns></returns>
        public static DataGrid GetDataGridFromChild(DependencyObject dataGridPart)
        {
            if (VisualTreeHelper.GetParent(dataGridPart) == null)
                throw new NullReferenceException("Control is null.");

            if (VisualTreeHelper.GetParent(dataGridPart) is DataGrid)
                return (DataGrid)VisualTreeHelper.GetParent(dataGridPart);
            else
                return GetDataGridFromChild(VisualTreeHelper.GetParent(dataGridPart));
        }
    }
}
