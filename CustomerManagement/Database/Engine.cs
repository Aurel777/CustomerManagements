namespace CustomerManagement.Database
{
    #region Using Statements

    using Model;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;

    #endregion

    public class DataBaseEngine : IDisposable
    {
        #region Private Fields

        string connectionString;
        Dictionary<string, List<string>> tableColumnNames = new Dictionary<string, List<string>>();
        readonly string databaseName;

        
        #endregion

        #region Constructors

        public DataBaseEngine(string dataBaseName )
        {
            databaseName = dataBaseName;
            InitializeConnectionString();
        }

        ~DataBaseEngine() => Dispose(false);

        #endregion
        
        #region Methods

        public int Add(string query) => ExecuteNonQuery(query);
        
        public int Delete(string query) => ExecuteNonQuery(query);
        
        public object ExecuteQuery(Action action, string query)
        {
            object result = null;

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();
                switch (action)
                {
                    case Action.Add:
                        result = command.ExecuteNonQuery();
                        break;
                    case Action.Delete:
                        result = command.ExecuteNonQuery();
                        break;
                    case Action.Update:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(action), action, null);
                }
                connection.Close();
            }

            return result;
        }

        public int ExecuteNonQuery(string query)
        {
            int result;

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();
                result = command.ExecuteNonQuery();
                connection.Close();
            }

            return result;
        }

        public object ExecuteScalar(string query)
        {
            object result;

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();
                result = command.ExecuteScalar();
                connection.Close();
            }

            return result;
        }

        public IEnumerable<Customer> Select(string query)
        {
            using(var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return new Customer(reader[1].ToString(), reader[2].ToString())
                    {
                        Id =  int.Parse(reader[0].ToString()),
                        Address = reader[3].ToString(),
                        PostCode = reader[4].ToString(),
                        Country = reader[5].ToString(),
                        PhoneNumber = reader[6].ToString(),
                        Email = reader[7].ToString()
                    };
                }
                connection.Close();
            }
        }

        public int Update(string tableName, string[] columnNames, string[] values, string whereClause)
        {
            if (columnNames.Length != values.Length)
                throw new MismatchingUpdateArgumentException("Columns and values must have the same amount of data.");

            var query = $"UPDATE {tableName} SET ";
            for (var i = 0; i < columnNames.Length; i++)
                query += $"{columnNames[i]}='{values[i]}', ";
            var end = query.LastIndexOf(", ", StringComparison.Ordinal);
            query = query.Remove(end, query.Length - end);
            query += whereClause;

            return ExecuteNonQuery(query);
        }

        #endregion

        #region Members

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources.
            }

            // free native resources if there are any.
        }

        #endregion

        void InitializeConnectionString()
        {
            try
            {
                if (ConfigurationManager.ConnectionStrings.Count < 1)
                    throw new ConnectionStringNotFoundException("There is no connection string stored.");

                connectionString = ConfigurationManager.ConnectionStrings[databaseName]?.ConnectionString;
                if (string.IsNullOrEmpty(connectionString))
                    throw new InvalidConnectionStringException(nameof(connectionString));
            }
            catch (Exception exception)
            {
                throw new OpenDatabaseException("Failed to open a connection with the database", exception);
            }
        }

        #endregion
    }
}