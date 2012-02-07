using System;
using System.Collections.Generic;
using HurksBestelSysteem.Domain;
using System.Data;
using MySql.Data.MySqlClient;
using HurksBestelSysteem.Database;
using System.Globalization;

namespace HurksBestelSysteem.DAO.MySQL
{
    public class MySQLProductDAO : ProductDAO
    {
        public bool AddProduct(Product p)
        {
            try
            {
                using (IDbConnection connection = MySQLDAOFactory.GetDatabase().CreateOpenConnection())
                {
                    CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
                    string priceString = p.price.ToString(culture);  //we store it as US-style decimal
                    string query = "INSERT INTO product (productname, productcode, description, price, pricetype) VALUES "
                    + "('" + p.productName.Trim().ToLower() + "'," + "'" + p.productCode.ToString() + "'," + "'" + p.description + "'," + "'" + priceString + "','" + p.priceType + "')";
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
    }
}
