using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;


namespace DevCatalyst.Data.Database
{
    public  class CustomSqlConnection
    {
        private readonly IDbConnection _connection;
        public CustomSqlConnection(string? connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public void Open()
        {
            ((SqlConnection)_connection).Open();
        }

        public IEnumerable<T> Query<T>(string query, object? param = null, IDbTransaction? transaction = null)
        {
            var results = _connection.Query<T>(query, param, transaction);
            return results;

        }

 

        public int ExecuteScaler(string query, object? param = null, IDbTransaction? transaction = null)
        {
            return _connection.ExecuteScalar<int>(query, param, transaction);
        }
        public int Execute(string query, object? param = null, IDbTransaction? transaction = null)
        {

            var result = _connection.Execute(query, param, transaction);
            return result;

        }

 

        public void Close()
        {
            _connection.Close();
        }

        public ConnectionState State()
        {
            return _connection.State;
        }

        public bool IsAvailable()
        {
            return _connection.State != ConnectionState.Connecting && _connection.State != ConnectionState.Executing &&
                   _connection.State != ConnectionState.Fetching;
        }

        public bool IsClosed()
        {
            return _connection.State == ConnectionState.Closed;
        }
    }
}
