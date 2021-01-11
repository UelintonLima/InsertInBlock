namespace System.Data.SqlClient.Attributes
{
    /// <summary>
    /// This class is an extension of System.Data.DataTable to facilitate map the database table
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class SqlColumnAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string FieldName { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldName"></param>
        public SqlColumnAttribute(string fieldName)
        {
            this.FieldName = fieldName;
        }
    }
}
