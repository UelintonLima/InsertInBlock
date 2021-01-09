using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.SqlClient;

namespace InsertInBlockTest
{
    [TestClass]
    public class SqlConnectionExtensionsTest
    {
        private const string connectionString = "Server=localhost\\SQLEXPRESS;Database=test; UID=sa; PWD=master;";


        [TestMethod]
        public void TestInsertInBlock()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                DataTable dados = connection.GetTableStructure("table_test");

                var dr = dados.NewRow();
                dr["name"] = "teste name";
                dados.Rows.Add(dr);

                connection.InsertInBlock(dados);

                dados.Clear();
                using (SqlCommand cmd = new SqlCommand(string.Format("SELECT count(*)  FROM table_test where name = 'teste name'"), connection))
                {
                    Assert.IsTrue((int)cmd.ExecuteScalar() > 0);
                }
                connection.Close();
            }
        }

        [TestMethod]
        public void TestInsertInBlockHeavyLoad()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                DataTable dados = connection.GetTableStructure("table_test");

                for (int i = 1; i <= 1000000; i++)
                {
                    var dr = dados.NewRow();
                    dr["name"] = "teste name " + i;
                    dados.Rows.Add(dr);
                }

                connection.InsertInBlock(dados);

                dados.Clear();
                using (SqlCommand cmd = new SqlCommand(string.Format("SELECT count(*)  FROM table_test where name = 'teste name 1000000' "), connection))
                {
                    Assert.IsTrue((int)cmd.ExecuteScalar() > 0);
                }
                connection.Close();
            }
        }
    }
}
