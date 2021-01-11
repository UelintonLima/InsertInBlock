# InsertInBlock
This project is an extension of System.Data.SqlClient to facilitate block data insertion.

## using System.Collections.Generic.List<class>

how to use mapping in the class:
```
[SqlTable(tableName: "person")]
public class Person
{
	public Person(string firstName, string lastName, int age)
	{
		this.FirstName = firstName;
		this.LastName = lastName;
		this.Age = age;
	}

	[SqlColumn(fieldName: "first_name")]
	public string FirstName { get; set; }

	[SqlColumn(fieldName: "last_name")]
	public string LastName { get; set; }

	[SqlColumn(fieldName: "age")]
	public int Age { get; set; }
}
```

how to use a list for data entry
```	
List<TableTest> dados = new List<TableTest>();
dados.Add(new TableTest() { Name = "João" });
dados.Add(new TableTest() { Name = "Maria" });
dados.Add(new TableTest() { Name = "Pedro" });

using (SqlConnection connection = new SqlConnection(connectionString))
{
	connection.Open();
	connection.InsertInBlock(dados);
	connection.Close();
}
```

## using System.Data.DataTable
```
using (SqlConnection connection = new SqlConnection(connectionString))
{
	connection.Open();

	DataTable dados = connection.GetTableStructure("table_test");

	var dr = dados.NewRow();
	dr["name"] = "João";
	dados.Rows.Add(dr);

	dr = dados.NewRow();
	dr["name"] = "Maria";
	dados.Rows.Add(dr);
	
	connection.InsertInBlock(dados);

	connection.Close();
}
```
