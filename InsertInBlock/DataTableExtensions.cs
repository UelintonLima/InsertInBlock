namespace System.Data.SqlClient
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

    }
}
