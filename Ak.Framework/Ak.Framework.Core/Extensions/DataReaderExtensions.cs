using System.Data;

namespace Ak.Framework.Core.Extensions
{
    /// <summary>
    /// Расширения для работы с DataReader'ом
    /// </summary>
    public static class DataReaderExtensions
    {
        /// <summary>
        /// Проверка на сущестование столбца (поля)
        /// </summary>
        /// <param name="reader">Data Reader</param>
        /// <param name="fieldName">Название поля (столбца)</param>
        /// <returns></returns>
        public static bool FieldExists(this IDataReader reader, string fieldName)
        {
            DataTable schemaTable = reader.GetSchemaTable();
            if (schemaTable == null)
                return false;
            schemaTable.DefaultView.RowFilter = $"ColumnName= '{fieldName}'";
            return (schemaTable.DefaultView.Count > 0);
        }
    }
}
