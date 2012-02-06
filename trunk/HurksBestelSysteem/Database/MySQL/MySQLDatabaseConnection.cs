using System.Data;
using MySql.Data.MySqlClient;

namespace HurksBestelSysteem.Database.MySQL
{
    class MySQLDatabaseConnection : DatabaseConnection
    {
        public override IDbConnection CreateConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public override IDbCommand CreateCommand()
        {
            return new MySqlCommand();
        }

        public override IDbConnection CreateOpenConnection()
        {
            MySqlConnection connection = (MySqlConnection)CreateConnection();
            connection.Open();

            return connection;
        }

        public override IDbCommand CreateCommand(string commandText, IDbConnection connection)
        {
            MySqlCommand command = (MySqlCommand)CreateCommand();

            command.CommandText = commandText;
            command.Connection = (MySqlConnection)connection;
            command.CommandType = CommandType.Text;

            return command;
        }

        public override IDbCommand CreateStoredProcCommand(string procName, IDbConnection connection)
        {
            MySqlCommand command = (MySqlCommand)CreateCommand();

            command.CommandText = procName;
            command.Connection = (MySqlConnection)connection;
            command.CommandType = CommandType.StoredProcedure;

            return command;
        }

        public override IDataParameter CreateParameter(string parameterName, object parameterValue)
        {
            return new MySqlParameter(parameterName, parameterValue);
        }
    }
}
