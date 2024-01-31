

namespace DevCatalyst.Data.Database
{
    public static class ConnectionManager
    {
        private static CustomSqlConnection? _connection;

        public static CustomSqlConnection GetConnection(string? cos)
        {
            if (_connection == null)
                _connection = OpenConnection(cos);

            if (_connection.IsClosed())
                _connection.Open();

            return _connection;
        }
        private static CustomSqlConnection OpenConnection(string? connectionString)
        {
            _connection = new CustomSqlConnection(connectionString);
            _connection.Open();
            return _connection;
        }
    }
}
