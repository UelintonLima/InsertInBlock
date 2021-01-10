using System.Data.SqlClient.Attributes;

namespace InsertInBlockTest.Entities
{
    [SqlTable(tableName: "table_test")]
    public class TableTest
    {
        [SqlColumn(fieldName: "name")]
        public string Name { get; set; }
    }
}
