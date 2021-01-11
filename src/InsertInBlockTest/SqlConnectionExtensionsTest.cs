using InsertInBlockTest.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace InsertInBlockTest
{
    [TestClass]
    public class SqlConnectionExtensionsTest
    {
        private const string connectionString = "Server=localhost\\SQLEXPRESS;Database=test; UID=sa; PWD=master;";

        [TestMethod]
        public void TestInsertInBlockClass()
        {
            List<TableTest> dados = new List<TableTest>();
            dados.Add(new TableTest() { Name = "João" });
            dados.Add(new TableTest() { Name = "Maria" });
            dados.Add(new TableTest() { Name = "Pedro" });

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.InsertInBlock(dados);
                connection.Close();

                using (SqlCommand cmd = new SqlCommand("SELECT count(*)  FROM table_test where name = 'João'", connection))
                {
                    Assert.IsTrue((int)cmd.ExecuteScalar() > 0);
                }
                using (SqlCommand cmd = new SqlCommand("SELECT count(*)  FROM table_test where name = 'Maria'", connection))
                {
                    Assert.IsTrue((int)cmd.ExecuteScalar() > 0);
                }
                using (SqlCommand cmd = new SqlCommand("SELECT count(*)  FROM table_test where name = 'Pedro'", connection))
                {
                    Assert.IsTrue((int)cmd.ExecuteScalar() > 0);
                }
            }
        }

        [TestMethod]
        public void TestInsertInBlockClassHeavyLoad()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                List<TableTest> dados = new List<TableTest>();

                for (int i = 1; i <= 1000000; i++)
                {
                    dados.Add(new TableTest() { Name = "teste name " + i });
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

        [TestMethod]
        public void TestInsertInBlockDataTable()
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
        public void TestInsertInBlockDataTableHeavyLoad()
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
