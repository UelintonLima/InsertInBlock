using System.Collections.Generic;
using System.Reflection;

namespace System.Data.SqlClient.Attributes
{
    /// <summary>
    /// This class is an extension of System.Data.SqlClient to facilitate custom attributes.
    /// </summary>
    public static class AttributeExtensions
    {
        /// <summary>
        ///     This method Convert System.Collections.Generic.List to System.Data.DataTable.
        /// </summary>
        /// <typeparam name="T">class to be mapped to a target System.Data.DataRow</typeparam>
        /// <param name="data">Destination System.Data.DataTable that will receive the System.Collections.Generic.List data</param>
        /// <returns>Returns a System.Data.DataTable filled with the System.Collections.Generic.List data</returns>
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

        /// <summary>
        ///     This method gets the TableName property of the System.Data.SqlClient.Attributes.SqlTableAttribute class
        ///     if the mapping is not found, the method returns the value of the class name
        /// </summary>        
        /// <typeparam name="T">Class to be mapped</typeparam>
        /// <param name="data">System.Collections.Generic.List class to be mapped</param>
        /// <returns>returns the TableName property of the System.Data.SqlClient.Attributes.SqlTableAttribute class
        /// if the mapping is not found, the method returns the value of the class name</returns>
        public static string GetTableName<T>(this List<T> data) where T : class
        {
            Type type = typeof(T);

            var attribute = type.GetCustomAttribute<SqlTableAttribute>();

            return attribute != null ? attribute.TableName : type.Name;
        }

        /// <summary>
        ///     This method gets the FieldName property of the System.Data.SqlClient.Attributes.SqlColumnAttribute class
        ///     if the mapping is not found, the method returns the value of the property name
        /// </summary>        
        /// <typeparam name="T">Class to be mapped</typeparam>
        /// <param name="data">System.Collections.Generic.List class to be mapped</param>
        /// <returns>returns the FieldName property of the System.Data.SqlClient.Attributes.SqlColumnAttribute class
        /// if the mapping is not found, the method returns the value of the property name</returns>
        public static string GetFieldName(this PropertyInfo property)
        {
            var attribute = property.GetCustomAttribute<SqlColumnAttribute>();

            return attribute != null ? attribute.ColumnName : property.Name;
        }
    }
}
