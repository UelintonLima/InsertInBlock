using InsertInBlockTest.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient.Attributes;

namespace InsertInBlockTest
{
    [TestClass]
    public class DataTableExtensionsTest
    {

        [TestMethod]
        public void TestConvertToDataTable()
        {
            var persons = new List<Person>();
            persons.Add(new Person("João", "paulo", 31));
            persons.Add(new Person("maria", "fernandes", 25));

            var tablePersons = persons.ConvertToDataTable();

            var tableCompare = new DataTable("person");
            tableCompare.Columns.Add("first_name");
            tableCompare.Columns.Add("last_name");
            tableCompare.Columns.Add("age", typeof(int));

            var dr = tableCompare.NewRow();
            dr["first_name"] = "João";
            dr["last_name"] = "paulo";
            dr["age"] = 31;
            tableCompare.Rows.Add(dr);

            dr = tableCompare.NewRow();
            dr["first_name"] = "maria";
            dr["last_name"] = "fernandes";
            dr["age"] = 25;
            tableCompare.Rows.Add(dr);

            Assert.AreEqual(tablePersons.Rows.Count, tableCompare.Rows.Count);
            Assert.AreEqual(tablePersons.Columns.Count, tableCompare.Columns.Count);

            Assert.AreEqual(tablePersons.Columns["first_name"].ColumnName, tableCompare.Columns["first_name"].ColumnName);
            Assert.AreEqual(tablePersons.Columns["last_name"].ColumnName, tableCompare.Columns["last_name"].ColumnName);
            Assert.AreEqual(tablePersons.Columns["age"].ColumnName, tableCompare.Columns["age"].ColumnName);

            Assert.AreEqual(tablePersons.Rows[0]["first_name"], tableCompare.Rows[0]["first_name"]);
            Assert.AreEqual(tablePersons.Rows[1]["first_name"], tableCompare.Rows[1]["first_name"]);

            Assert.AreEqual(tablePersons.Rows[0]["last_name"], tableCompare.Rows[0]["last_name"]);
            Assert.AreEqual(tablePersons.Rows[1]["last_name"], tableCompare.Rows[1]["last_name"]);

            Assert.AreEqual(tablePersons.Rows[0]["age"], tableCompare.Rows[0]["age"]);
            Assert.AreEqual(tablePersons.Rows[1]["age"], tableCompare.Rows[1]["age"]);

            Assert.AreEqual(tablePersons.Columns["first_name"].DataType, tableCompare.Columns["first_name"].DataType);
            Assert.AreEqual(tablePersons.Columns["last_name"].DataType, tableCompare.Columns["last_name"].DataType);
            Assert.AreEqual(tablePersons.Columns["age"].DataType, tableCompare.Columns["age"].DataType);
        }

        [TestMethod]
        public void TestConvertToDataTableHeavyLoad()
        {

            List<TableTest> dados = new List<TableTest>();

            for (int i = 1; i <= 1000000; i++)
            {
                dados.Add(new TableTest() { Name = "teste name " + i });
            }

            var table = dados.ConvertToDataTable();

            Assert.AreEqual(table.Rows.Count, 1000000);
        }
    }
}
