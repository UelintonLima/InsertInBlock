using System.Collections.Generic;
using System.Data.SqlClient.Attributes;
using System.Reflection;

namespace System.Data
{
    public static class AttributeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetTableName<T>(this List<T> data) where T : class
        {
            Type type = typeof(T);

            var attribute = type.GetCustomAttribute<SqlTableAttribute>();

            return attribute != null ? attribute.TableName : type.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string GetFieldName(this PropertyInfo property)
        {
            var attribute = property.GetCustomAttribute<SqlColumnAttribute>();

            return attribute != null ? attribute.FieldName : property.Name;
        }
    }
}
