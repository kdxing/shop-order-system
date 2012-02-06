using System.Data;

namespace HurksBestelSysteem.Database
{
    public abstract class DatabaseConnection
    {
        public string connectionString;

        public abstract IDbConnection CreateConnection();
        public abstract IDbCommand CreateCommand();
        public abstract IDbConnection CreateOpenConnection();
        public abstract IDbCommand CreateCommand(string commandText, IDbConnection connection);
        public abstract IDbCommand CreateStoredProcCommand(string procName, IDbConnection connection);
        public abstract IDataParameter CreateParameter(string parameterName, object parameterValue);
    }
}
