# InsertInBlock
This project is an extension of System.Data.SqlClient to facilitate block data insertion.

```
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();

    DataTable dados = connection.GetTableStructure("table_test");

    var dr = dados.NewRow();
    dr["name"] = "teste name";
    dados.Rows.Add(dr);

    connection.InsertInBlock(dados);

    connection.Close();
}
```
