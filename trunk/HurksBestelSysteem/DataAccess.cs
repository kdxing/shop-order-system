using System;
using HurksBestelSysteem.Domain;
using HurksBestelSysteem.DAO;
using HurksBestelSysteem.Database;

namespace HurksBestelSysteem
{
    public class DataAccess
    {
        private static DAOFactory daoFactory;

        public DataAccess()
        {
            if (daoFactory == null)
            {
                daoFactory = DAOFactory.GetDAOFactory(DAOFactory.FactoryType.MySQL);
            }
        }

        //#################### PRODUCT FUNCTIONS ###########################
        //#################### PRODUCT FUNCTIONS ###########################
        #region PRODUCT_FUNCTIONS

        //returnt false als de gegevens niet kloppen
        //returnt true als de gegevens wel kloppen en de klant is toegevoegd
        public bool AddProduct(Product product)
        {
            ProductDAO dao = daoFactory.GetProductDAO();
            try
            {
                return dao.AddProduct(product);
            }
            catch (DatabaseException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GetProductByCode(int productCode, out Product product)
        {
            ProductDAO dao = daoFactory.GetProductDAO();
            try
            {
                return dao.GetProductByCode(productCode, out product);
            }
            catch (DatabaseException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GetProductsByName(string productName, out Product[] products)
        {
            ProductDAO dao = daoFactory.GetProductDAO();
            try
            {
                return dao.GetProductsByName(productName, out products);
            }
            catch (DatabaseException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool RemoveProduct(Product p)
        {
            ProductDAO dao = daoFactory.GetProductDAO();
            try
            {
                return dao.RemoveProduct(p);
            }
            catch (DatabaseException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion PRODUCT_FUNCTIONS
        //#################### END OF PRODUCT FUNCTIONS ####################
        //#################### END OF PRODUCT FUNCTIONS ####################
    }
}
