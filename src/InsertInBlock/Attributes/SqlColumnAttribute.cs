namespace System.Data.SqlClient.Attributes
{
    /// <summary>
    /// This class is a custom attribute to map as columns in a database table
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class SqlColumnAttribute : Attribute
    {
        public SqlColumnAttribute(string columnName)
        {
            this.ColumnName = columnName;
        }

        /// <summary>
        ///     Column name in the database
        /// </summary>
        public string ColumnName { get; private set; }

    }
}
