using System.Collections.Generic;
using System.Reflection;

namespace System.Data
{
    /// <summary>
    /// This class is an extension of System.Data.DataTable to facilitate inclusion of records
    /// </summary>
    public static class DataTableExtensions
    {
        /// <summary>
        ///     This method adds a System.Data.DataRow to the target System.Data.DataTable with existing columns
        /// </summary>
        /// <param name="data">A System.Data.DataTable whose line will be added.</param>
        /// <param name="row">A System.Data.DataRow line that will be added to the target System.Data.DataTable.</param>
        public static void AddRow(this DataTable data, DataRow row)
        {
            DataRow newRow = data.NewRow();
            foreach (DataColumn column in data.Columns)
            {
                if (row.Table.Columns.IndexOf(column.ColumnName) != -1)
                    newRow[column.ColumnName] = row[column.ColumnName];
            }
            data.Rows.Add(newRow);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DataTable ConvertToDataTable<T>(this List<T> data) where T : class
        {
            var table = new DataTable(data.GetTableName());

            PropertyInfo[] properties = typeof(T).GetProperties();

            var colluns = new Dictionary<string, string>();
            foreach (var property in properties)
            {
                var fieldName = property.GetFieldName();
                colluns.Add(property.Name, fieldName);
                table.Columns.Add(fieldName, property.PropertyType);
            }

            DataRow dr;
            foreach (var row in data)
            {
                dr = table.NewRow();

                foreach (var property in properties)
                    dr[colluns[property.Name]] = property.GetValue(row);

                table.Rows.Add(dr);
            }

            return table;
        }
    }
}
