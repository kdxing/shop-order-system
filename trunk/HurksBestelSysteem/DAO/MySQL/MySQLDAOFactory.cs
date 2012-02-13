using HurksBestelSysteem.Database;
using HurksBestelSysteem.Database.MySQL;

namespace HurksBestelSysteem.DAO.MySQL
{
    class MySQLDAOFactory : DAOFactory
    {
        private static DatabaseConnection database;
        private const string host = "localhost";
        private const string userName = "root";
        private const string password = "password";
        private const int port = 22;
        private static readonly string databaseName = "hurksbestelsysteem";

        public MySQLDAOFactory() 
        {
            GetDatabase();
        }

        public static string DatabaseName
        {
            get { return databaseName; }
        }

        public static DatabaseConnection GetDatabase()
        {
            if (database == null)
            {
                database = new MySQLDatabaseConnection();
                database.connectionString = ("Server=" + host + ";Database= " + databaseName + ";Uid=" + userName + ";Pwd=" + password + ";");  
            }
            return database;
        }

        public override ProductDAO GetProductDAO()
        {
            return new MySQLProductDAO();
        }

        public override CategoryDAO GetCategoryDAO()
        {
            return new MySQLCategoryDAO();
        }

        public override OrderDAO GetOrderDAO()
        {
            return new MySQLOrderDAO();
        }

        public override CustomerDAO GetCustomerDAO()
        {
            return new MySQLCustomerDAO();
        }
    }
}
