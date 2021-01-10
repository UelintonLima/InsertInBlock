using System.Collections.Generic;
using System.Data.SqlClient.Attributes;
using System.Reflection;

namespace System.Data
{
    public static class AttributeExtensions
    {
        public static string GetTableName<T>(this List<T> data) where T : class
        {
            var tableName = nameof(T);

            Type type = typeof(T);
            if (type.IsDefined(typeof(SqlTableAttribute), true))
            {
                var attribute = type.GetCustomAttribute<SqlTableAttribute>();
                tableName = attribute.TableName;
            }
            return tableName;
        }

        public static string GetFieldName(this PropertyInfo property)
        {
            var fieldName = property.Name;

            if (property.IsDefined(typeof(SqlColumnAttribute), true))
            {
                var attribute = property.GetCustomAttribute<SqlColumnAttribute>();
                fieldName = attribute.FieldName;
            }
            return fieldName;
        }
    }
}
