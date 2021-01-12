namespace System.Data.SqlClient.Attributes
{
    /// <summary>
    /// This class is a custom attribute for mapping a database table
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class SqlTableAttribute : Attribute
    {
        public SqlTableAttribute(string tableName)
        {
            this.TableName = tableName;
        }
        /// <summary>
        ///     Table name in the database
        /// </summary>
        public string TableName { get; private set; }
    }
}
