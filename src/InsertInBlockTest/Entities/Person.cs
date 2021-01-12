using System.Data.SqlClient.Attributes;

namespace InsertInBlockTest.Entities
{
    [SqlTable(tableName: "person")]
    public class Person
    {
        public Person(string firstName, string lastName, int age)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Age = age;
        }

        [SqlColumn(columnName: "first_name")]
        public string FirstName { get; set; }

        [SqlColumn(columnName: "last_name")]
        public string LastName { get; set; }

        [SqlColumn(columnName: "age")]
        public int Age { get; set; }
    }
}
