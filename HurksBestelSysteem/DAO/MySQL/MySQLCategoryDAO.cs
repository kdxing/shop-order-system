using System;
using System.Collections.Generic;
using HurksBestelSysteem.Domain;
using System.Data;
using MySql.Data.MySqlClient;
using HurksBestelSysteem.Database;
using System.Text;

namespace HurksBestelSysteem.DAO.MySQL
{
    public class MySQLCategoryDAO : CategoryDAO
    {
        public bool GetAllCategories(out ProductCategory[] categories)
        {
            try
            {
                using (IDbConnection connection = MySQLDAOFactory.GetDatabase().CreateOpenConnection())
                {
                    string query = "SELECT * FROM category";
                    using (IDbCommand command = MySQLDAOFactory.GetDatabase().CreateCommand(query, connection))
                    {
                        using (IDataReader reader = command.ExecuteReader())
                        {
                            List<ProductCategory> categoryList = new List<ProductCategory>();
                            while (reader.Read())
                            {
                                ProductCategory c = new ProductCategory(
                                    reader["categoryname"].ToString(),
                                    reader["categorydescription"].ToString(),
                                    Convert.ToInt32(reader["idcategory"])
                                    );
                                categoryList.Add(c);
                            }
                            categories = categoryList.ToArray();
                            if (categories.Length > 0)
                            {
                                return true;
                            }
                            return false;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new DatabaseException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool AddCategory(ProductCategory category)
        {
            using (IDbConnection connection = MySQLDAOFactory.GetDatabase().CreateOpenConnection())
            {
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = "INSERT INTO category (categoryname, categorydescription) VALUES "
                        + "('" + category.name.Trim().ToLower() + "', '" + category.description + "')";
                        using (IDbCommand command = MySQLDAOFactory.GetDatabase().CreateCommand(query, connection))
                        {
                            if (command.ExecuteNonQuery() <= 0)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }
                        string queryID = "SELECT LAST_INSERT_ID();";
                        using (IDbCommand commandID = MySQLDAOFactory.GetDatabase().CreateCommand(queryID, connection))
                        {
                            using (IDataReader reader = commandID.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    category.internalID = Convert.ToInt32(reader[0]);
                                }
                            }
                            if (category.internalID.Equals(-1) == false)
                            {
                                transaction.Commit();
                                return true;
                            }
                            else
                            {
                                transaction.Rollback();
                                throw new Exception("category inserted succesfully, but could not retrieve ID afterwards. Rolled back.");
                            }
                        }
                    }
                    catch (MySqlException ex)
                    {
                        transaction.Rollback();
                        throw new DatabaseException(ex.Message, ex);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public bool RemoveCategory(ProductCategory category)
        {
            return false;
        }

    }
}
