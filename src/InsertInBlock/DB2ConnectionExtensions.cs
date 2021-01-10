using System;
using System.Collections.Generic;
using System.Data;

namespace IBM.Data.DB2
{
    /// <summary>
    /// This class is an extension of IBM.Data.DB2 to facilitate block data insertion.
    /// </summary>
    public static class DB2ConnectionExtensions
    {
        /// <summary>
        ///     This method inserts block data.
        /// </summary>
        /// <param name="connection">The already open IBM.Data.DB2.DB2Connection instance that will be used.</param>
        /// <param name="data">A System.Data.DataTable whose rows will be copied to the target table. 
        /// The TableName property must be filled in with the name of the destination table.</param>
        public static void InsertInBlock(this DB2Connection connection, DataTable data) => connection.InsertInBlock(data: data, tableName: data.TableName);

        /// <summary>
        ///     This method inserts block data.
        /// </summary>
        /// <param name="connection">The already open IBM.Data.DB2.DB2Connection instance that will be used.</param>
        /// <param name="data">A System.Data.DataTable whose rows will be copied to the destination table.</param>
        /// <param name="tableName">The name of the table that will be inserted into the data.</param>
        public static void InsertInBlock(this DB2Connection connection, DataTable data, string tableName) => connection.InsertInBlock(data: data, tableName: tableName);


        /// <summary>
        ///     This method inserts block data.
        /// </summary>
        /// <param name="connection">The already open IBM.Data.DB2.DB2Connection instance that will be used.</param>
        /// <param name="data"></param>
        /// <param name="tableName">The name of the table that will be inserted into the data.</param>
        public static void InsertInBlock<T>(this DB2Connection connection, List<T> data) where T : class
        {
            connection.InsertInBlock(data: data.ConvertToDataTable(), tableName: data.GetTableName());
        }

        /// <summary>
        ///     This method inserts block data.
        /// </summary>
        /// <param name="connection">The already open IBM.Data.DB2.DB2Connection instance that will be used.</param>
        /// <param name="data"></param>
        /// <param name="tableName">The name of the table that will be inserted into the data.</param>
        public static void InsertInBlock<T>(this DB2Connection connection, List<T> data, string tableName) where T : class
        {
            connection.InsertInBlock(data: data.ConvertToDataTable(), tableName: tableName);
        }

        /// <summary>
        ///     This method inserts block data.
        /// </summary>
        /// <param name="connection">The already open IBM.Data.DB2.DB2Connection instance that will be used.</param>
        /// <param name="data">A System.Data.DataTable whose rows will be copied to the destination table.</param>
        /// <param name="tableName">The name of the table that will be inserted into the data.</param>
        /// <param name="bulkCopyOptions">Bitwise flag that specifies one or more options to use with an instance of IBM.Data.DB2.DB2BulkCopy.</param>
        /// <param name="batchSize">Number of rows in each batch. At the end of each batch, the rows in the batch are sent to the server.
        /// The integer value of the IBM.Data.DB2.DB2BulkCopy.NotifyAfter property, or zero if the property has not been set.</param>
        /// <param name="bulkCopyTimeout">Number of seconds for the operation to complete before it times out.
        /// The default is 30 seconds. A value of 0 indicates no limit; the bulk copy will</param>
        /// <param name="notifyAfter">Defines the number of rows to be processed before generating a notification event.
        /// IBM.Data.DB2.DB2BulkCopy.NotifyAfter property, or zero if the property has not been set.</param>       
        public static void InsertInBlock(this DB2Connection connection, DataTable data, string tableName, DB2BulkCopyOptions bulkCopyOptions = DB2BulkCopyOptions.TableLock, int batchSize = 0, int bulkCopyTimeout = 30, int notifyAfter = 0)
        {
            if (tableName == "")
                throw new Exception(string.Format("Parameter {0} cannot be null", nameof(tableName)));
            if (connection.State != ConnectionState.Open)
                throw new Exception("Connection is not open");

            var table = connection.GetTableStructure(tableName);

            foreach (DataRow row in data.Rows)
                table.AddRow(row);

            using (DB2BulkCopy bulk = new DB2BulkCopy(connection, bulkCopyOptions))
            {
                bulk.BulkCopyTimeout = bulkCopyTimeout;
                bulk.NotifyAfter = notifyAfter;
                bulk.DestinationTableName = tableName;
                bulk.WriteToServer(table);
            }
        }

        /// <summary>
        ///     This method gets the structure of the database table.
        /// </summary>
        /// <param name="connection">The already open IBM.Data.DB2.DB2Connection instance that will be used.</param>
        /// <param name="tableName">The name of the table that will be inserted into the data.</param>
        /// <returns>the table structure in System.Data.DataTable</returns>
        public static DataTable GetTableStructure(this DB2Connection connection, string tableName)
        {
            DataTable dt = new DataTable();

            using (DB2Command cmd = new DB2Command(string.Format("SELECT TOP 0 * FROM {0}", tableName), connection))
            {
                using (DB2DataAdapter dr = new DB2DataAdapter(cmd))
                {
                    dr.FillSchema(dt, SchemaType.Source);
                }
            }

            return dt;
        }
    }
}
