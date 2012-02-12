using HurksBestelSysteem.DAO.MySQL;

namespace HurksBestelSysteem.DAO
{
    public abstract class DAOFactory
    {
        public enum FactoryType { MySQL }

        public abstract ProductDAO GetProductDAO();
        public abstract CategoryDAO GetCategoryDAO();
        public abstract OrderDAO GetOrderDAO();
        public abstract CustomerDAO GetCustomerDAO();

        public static DAOFactory GetDAOFactory(FactoryType factoryType)
        {
            switch (factoryType)
            {
                case FactoryType.MySQL:
                    return new MySQLDAOFactory();
                default:
                    return null;
            }
        }
    }
}
