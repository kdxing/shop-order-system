using System;
using System.Collections.Generic;
using HurksBestelSysteem.Domain;
using System.Data;
using MySql.Data.MySqlClient;
using HurksBestelSysteem.Database;
using System.Globalization;
using System.Text;

namespace HurksBestelSysteem.DAO.MySQL
{
    public class MySQLProductDAO : ProductDAO
    {
        public bool AddProduct(Product p)
        {
            if (p.internalID.Equals(-1) == false)
            {
                throw new Exception("Product to insert into database already has database id!");
            }

            using (IDbConnection connection = MySQLDAOFactory.GetDatabase().CreateOpenConnection())
            {
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
                        string priceString = p.price.ToString(culture);  //we store it as US-style decimal
                        string productName = p.productName.Trim().ToLower();
                        string query = "INSERT INTO product (productname, productcode, description, price, pricetype) VALUES "
                        + "('" + productName + "'," + "'" + p.productCode.ToString() + "'," + "'" + p.description + "'," + "'" + priceString + "','" + p.priceType + "')";
                        using (IDbCommand command = MySQLDAOFactory.GetDatabase().CreateCommand(query, connection))
                        {
                            if (command.ExecuteNonQuery() <= 0)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }
                        if (p.categories != null && p.categories.Length > 0)
                        {
                            //now get id
                            string queryID = "SELECT LAST_INSERT_ID();";
                            using (IDbCommand commandID = MySQLDAOFactory.GetDatabase().CreateCommand(queryID, connection))
                            {
                                using (IDataReader reader = commandID.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        p.internalID = Convert.ToInt32(reader[0]);
                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                        throw new Exception("product inserted succesfully, but could not retrieve ID afterwards");
                                    }
                                }
                            }
                            if (p.internalID.Equals(-1) == false)
                            {
                                //we have the id, now add categories
                                StringBuilder catQuery = new StringBuilder("INSERT INTO product_category (categoryid, productid) VALUES ", p.categories.Length * 2);
                                for (int i = 0; i < p.categories.Length; i++)
                                {
                                    catQuery.Append("(" + p.categories[i].internalID + ", " + p.internalID + ")");
                                    if (i != (p.categories.Length - 1))
                                    {
                                        catQuery.Append(", ");
                                    }
                                }
                                using (IDbCommand commandCat = MySQLDAOFactory.GetDatabase().CreateCommand(catQuery.ToString(), connection))
                                {
                                    if (commandCat.ExecuteNonQuery() <= 0)
                                    {
                                        transaction.Rollback();
                                        return false;
                                    }
                                    else
                                    {
                                        transaction.Commit();
                                        return true;
                                    }
                                }
                            }
                            else
                            {
                                transaction.Rollback();
                                throw new Exception("product inserted succesfully, but retrieved id was -1");
                            }
                        }
                        else
                        {
                            transaction.Commit();
                            return true;
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

        public bool GetProductByCode(int productCode, out Product product)
        {
            try
            {
                using (IDbConnection connection = MySQLDAOFactory.GetDatabase().CreateOpenConnection())
                {
                    string query = "SELECT * FROM product WHERE product.productcode = '" + productCode.ToString() + "'"; 
                    using (IDbCommand command = MySQLDAOFactory.GetDatabase().CreateCommand(query, connection))
                    {
                        using (IDataReader reader = command.ExecuteReader())
                        {
                            NumberFormatInfo numberInfo = System.Globalization.NumberFormatInfo.CurrentInfo;
                            Product p = null;
                            if (reader.Read())
                            {
                                p = new Product(
                                    reader["productname"].ToString(),
                                    productCode,
                                    reader["description"].ToString(),
                                    Decimal.Parse(reader["price"].ToString(), NumberStyles.Currency, numberInfo),
                                    ((Product.PriceType)(Convert.ToInt32(reader["pricetype"]))),
                                    null, //TODO
                                    (Convert.ToInt32(reader["idproduct"]))
                                    );
                            }
                            product = p;
                            if (product != null)
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

        public bool GetProductsByName(string productName, out Product[] products)
        {
            if (productName.Equals(""))
            {
                products = new Product[0];
                return false;
            }
            try
            {
                using (IDbConnection connection = MySQLDAOFactory.GetDatabase().CreateOpenConnection())
                {
                    string query = "SELECT * FROM product WHERE product.productname LIKE '%" + productName.Trim().ToLower() + "%'";
                    using (IDbCommand command = MySQLDAOFactory.GetDatabase().CreateCommand(query, connection))
                    {
                        using (IDataReader reader = command.ExecuteReader())
                        {
                            NumberFormatInfo numberInfo = System.Globalization.NumberFormatInfo.CurrentInfo;
                            List<Product> productsList = new List<Product>();
                            while (reader.Read())
                            {
                                Product.PriceType priceType;
                                if (Enum.TryParse<Product.PriceType>(reader["pricetype"].ToString(), true, out priceType))
                                {
                                    Product p = new Product(
                                        reader["productname"].ToString(),
                                        Convert.ToInt32(reader["productcode"]),
                                        reader["description"].ToString(),
                                        Decimal.Parse(reader["price"].ToString(), NumberStyles.Currency, numberInfo),
                                        priceType,
                                        null, //TODO
                                        (Convert.ToInt32(reader["idproduct"]))
                                        );
                                    productsList.Add(p);
                                }
                                else
                                {
                                    throw new Exception("Couldn't cast product price type enum!");
                                }
                            }
                            products = productsList.ToArray();
                            if (products.Length > 0)
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

        public bool RemoveProduct(Product p)
        {
            if (p.internalID == -1)
            {
                return false;
            }

            try
            {
                using (IDbConnection connection = MySQLDAOFactory.GetDatabase().CreateOpenConnection())
                {
                    //upon deletion, make sure both the productcode and the internal database id match, that way we're 100% sure the right product is deleted
                    string query = "DELETE FROM product WHERE product.productcode = '" + p.productCode.ToString() + "' AND product.idproduct = '" + p.internalID.ToString() + "'";
                    using (IDbCommand command = MySQLDAOFactory.GetDatabase().CreateCommand(query, connection))
                    {
                        if (command.ExecuteNonQuery() <= 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
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

        public bool GetProductsByCategory(ProductCategory[] categories, out Product[] products)
        {
            try
            {
                using (IDbConnection connection = MySQLDAOFactory.GetDatabase().CreateOpenConnection())
                {
                    StringBuilder query = new StringBuilder(
                        "SELECT * " +
                        "FROM " +
                        "(" +
                            "SELECT p.* " +
                            "FROM product AS p " +
                            "JOIN product_category AS cat " +
                            "ON (cat.productid = p.idproduct AND ("
                    );
                    for (int i = 0; i < categories.Length; i++)
                    {
                        if (i != 0)
                        {
                            query.Append("OR ");
                        }
                        query.Append("cat.categoryid = " + categories[i].internalID + " ");
                    }
                    query.Append(
                        "))" +
                        ")" +
                        "AS product_with_categories " +
                        "GROUP BY idproduct " +
                        "HAVING COUNT(idproduct) >= " + categories.Length
                        );
                    using (IDbCommand command = MySQLDAOFactory.GetDatabase().CreateCommand(query.ToString(), connection))
                    {
                        using (IDataReader reader = command.ExecuteReader())
                        {
                            NumberFormatInfo numberInfo = System.Globalization.NumberFormatInfo.CurrentInfo;
                            List<Product> productsList = new List<Product>();
                            while (reader.Read())
                            {
                                Product.PriceType priceType;
                                if (Enum.TryParse<Product.PriceType>(reader["pricetype"].ToString(), true, out priceType))
                                {
                                    Product p = new Product(
                                        reader["productname"].ToString(),
                                        Convert.ToInt32(reader["productcode"]),
                                        reader["description"].ToString(),
                                        Decimal.Parse(reader["price"].ToString(), NumberStyles.Currency, numberInfo),
                                        priceType,
                                        categories,
                                        (Convert.ToInt32(reader["idproduct"]))
                                        );
                                    productsList.Add(p);
                                }
                                else
                                {
                                    throw new Exception("Couldn't cast product price type enum!");
                                }
                            }
                            products = productsList.ToArray();
                            if (products.Length > 0)
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
    }
}
