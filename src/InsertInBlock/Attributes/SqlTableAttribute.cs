namespace System.Data.SqlClient.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class SqlTableAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string TableName { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        public SqlTableAttribute(string tableName)
        {
            this.TableName = tableName;
        }
    }
}
