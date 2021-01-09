namespace System.Data.SqlClient
{
    /// <summary>
    /// This class is an extension of System.Data.SqlClient to facilitate block data insertion.
    /// </summary>
    public static class SqlConnectionExtensions
    {
        /// <summary>
        ///     This method inserts block data.
        /// </summary>
        /// <param name="connection">The already open System.Data.SqlClient.SqlConnection instance that will be used.</param>
        /// <param name="data">A System.Data.DataTable whose rows will be copied to the target table. 
        /// The TableName property must be filled in with the name of the destination table.</param>
        public static void InsertInBlock(this SqlConnection connection, DataTable data) => connection.InsertInBlock(transaction: null, data: data, tableName: data.TableName);

        /// <summary>
        ///     This method inserts block data.
        /// </summary>
        /// <param name="connection">The already open System.Data.SqlClient.SqlConnection instance that will be used.</param>
        /// <param name="data">A System.Data.DataTable whose rows will be copied to the destination table.</param>
        /// <param name="tableName">The name of the table that will be inserted into the data.</param>
        public static void InsertInBlock(this SqlConnection connection, DataTable data, string tableName) => connection.InsertInBlock(transaction: null, data: data, tableName: tableName);

        /// <summary>
        ///     This method inserts block data.
        /// </summary>
        /// <param name="connection">The already open System.Data.SqlClient.SqlConnection instance that will be used.</param>
        /// <param name="transaction">An existing System.Data.SqlClient.SqlTransaction instance under which the bulk copy will occur.</param>
        /// <param name="data">A System.Data.DataTable whose rows will be copied to the target table. 
        /// The TableName property must be filled in with the name of the destination table.</param>        
        public static void InsertInBlock(this SqlConnection connection, SqlTransaction transaction, DataTable data) => connection.InsertInBlock(transaction: transaction, data: data, tableName: data.TableName);


        /// <summary>
        ///     This method inserts block data.
        /// </summary>
        /// <param name="connection">The already open System.Data.SqlClient.SqlConnection instance that will be used.</param>
        /// <param name="transaction">An existing System.Data.SqlClient.SqlTransaction instance under which the bulk copy will occur.</param>
        /// <param name="data">A System.Data.DataTable whose rows will be copied to the destination table.</param>
        /// <param name="tableName">The name of the table that will be inserted into the data.</param>
        /// <param name="bulkCopyOptions">Bitwise flag that specifies one or more options to use with an instance of System.Data.SqlClient.SqlBulkCopy.</param>
        /// <param name="batchSize">Number of rows in each batch. At the end of each batch, the rows in the batch are sent to the server.
        /// The integer value of the System.Data.SqlClient.SqlBulkCopy.NotifyAfter property, or zero if the property has not been set.</param>
        /// <param name="bulkCopyTimeout">Number of seconds for the operation to complete before it times out.
        /// The default is 30 seconds. A value of 0 indicates no limit; the bulk copy will</param>
        /// <param name="notifyAfter">Defines the number of rows to be processed before generating a notification event.
        /// System.Data.SqlClient.SqlBulkCopy.NotifyAfter property, or zero if the property has not been set.</param>       
        public static void InsertInBlock(this SqlConnection connection, SqlTransaction transaction, DataTable data, string tableName, SqlBulkCopyOptions bulkCopyOptions = SqlBulkCopyOptions.TableLock, int batchSize = 0, int bulkCopyTimeout = 30, int notifyAfter = 0)
        {
            if (tableName == "")
                throw new Exception(string.Format("Parameter {0} cannot be null", nameof(tableName)));
            if (connection.State != ConnectionState.Open)
                throw new Exception("Connection is not open");

            var table = connection.GetTableStructure(transaction, tableName);

            foreach (DataRow row in data.Rows)
                table.AddRow(row);

            using (SqlBulkCopy bulk = new SqlBulkCopy(connection, bulkCopyOptions, transaction))
            {
                bulk.BatchSize = batchSize;
                bulk.BulkCopyTimeout = bulkCopyTimeout;
                bulk.NotifyAfter = notifyAfter;
                bulk.DestinationTableName = tableName;
                bulk.WriteToServer(table);
            }
        }

        /// <summary>
        ///     This method gets the structure of the database table.
        /// </summary>
        /// <param name="connection">The already open System.Data.SqlClient.SqlConnection instance that will be used.</param>
        /// <param name="tableName">The name of the table that will be inserted into the data.</param>
        /// <returns>the table structure in System.Data.DataTable</returns>
        public static DataTable GetTableStructure(this SqlConnection connection, string tableName) => connection.GetTableStructure(transaction: null, tableName: tableName);

        /// <summary>
        ///     This method gets the structure of the database table.
        /// </summary>
        /// <param name="connection">The already open System.Data.SqlClient.SqlConnection instance that will be used.</param>
        /// <param name="transaction">An existing System.Data.SqlClient.SqlTransaction instance under which the bulk copy will occur.</param>
        /// <param name="tableName">The name of the table that will be inserted into the data.</param>
        /// <returns>the table structure in System.Data.DataTable</returns>
        public static DataTable GetTableStructure(this SqlConnection connection, SqlTransaction transaction, string tableName)
        {
            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand(string.Format("SELECT TOP 0 * FROM {0}", tableName), connection, transaction))
            {
                using (SqlDataAdapter dr = new SqlDataAdapter(cmd))
                {
                    dr.FillSchema(dt, SchemaType.Source);
                }
            }

            return dt;
        }
    }
}
