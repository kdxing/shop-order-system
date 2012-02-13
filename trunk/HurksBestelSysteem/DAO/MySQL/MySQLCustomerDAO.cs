using System;
using System.Collections.Generic;
using HurksBestelSysteem.Domain;
using System.Data;
using MySql.Data.MySqlClient;
using HurksBestelSysteem.Database;
using System.Text;

namespace HurksBestelSysteem.DAO.MySQL
{
    public class MySQLCustomerDAO : CustomerDAO
    {
        public bool UpdateCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        public bool AddCustomer(Customer customer)
        {
            if (customer.internalID.Equals(-1) == false)
            {
                throw new Exception("Customer to insert into database already has database id!");
            }

            using (IDbConnection connection = MySQLDAOFactory.GetDatabase().CreateOpenConnection())
            {
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = "INSERT INTO customer (firstname, lastname, phonenumber, street, housenumber, town) VALUES "
                        + "('" + customer.firstName + "', '" + customer.lastName + "', '" + customer.phoneNumber + "', '" + customer.street + "', '" + customer.houseNumber + "', '" + customer.town + "')";
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
                                    customer.internalID = Convert.ToInt32(reader[0]);
                                }
                            }
                        }
                        if (customer.internalID.Equals(-1))
                        {
                            transaction.Rollback();
                            throw new Exception("Customer inserted succesfully, but could not retrieve ID afterwards. Rollback.");
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

        public bool GetAllCustomers(out Customer[] customers)
        {
            try
            {
                using (IDbConnection connection = MySQLDAOFactory.GetDatabase().CreateOpenConnection())
                {
                    string query = "SELECT * FROM customer";
                    using (IDbCommand command = MySQLDAOFactory.GetDatabase().CreateCommand(query, connection))
                    {
                        using (IDataReader reader = command.ExecuteReader())
                        {
                            List<Customer> customerList = new List<Customer>();
                            while (reader.Read())
                            {
                                Customer c = new Customer(
                                    reader["firstname"].ToString(),
                                    reader["lastname"].ToString(),
                                    reader["phonenumber"].ToString(),
                                    reader["street"].ToString(),
                                    reader["housenumber"].ToString(),
                                    reader["town"].ToString(),
                                    Convert.ToInt32(reader["idcustomer"])
                                    );
                                customerList.Add(c);
                            }
                            customers = customerList.ToArray();
                            if (customers.Length > 0)
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

        public bool GetCustomerByID(int customerID, out Customer customer)
        {
            if (customerID.Equals(-1))
            {
                customer = null;
                return false;
            }
            try
            {
                using (IDbConnection connection = MySQLDAOFactory.GetDatabase().CreateOpenConnection())
                {
                    string query = "SELECT * FROM customer where customer.idcustomer = '" + customerID.ToString() + "'";
                    using (IDbCommand command = MySQLDAOFactory.GetDatabase().CreateCommand(query, connection))
                    {
                        using (IDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                customer = new Customer(
                                    reader["firstname"].ToString(),
                                    reader["lastname"].ToString(),
                                    reader["phonenumber"].ToString(),
                                    reader["street"].ToString(),
                                    reader["housenumber"].ToString(),
                                    reader["town"].ToString(),
                                    customerID
                                    );
                                return true;
                            }
                            customer = null;
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

        public bool GetCustomersByName(string customerName, out Customer[] customers)
        {
            string searchName = customerName.Trim().ToLower();
            if (searchName.Equals(""))
            {
                customers = new Customer[0];
                return false;
            }
            try
            {
                using (IDbConnection connection = MySQLDAOFactory.GetDatabase().CreateOpenConnection())
                {
                    string query = "SELECT * FROM customer where customer.firstname LIKE '%" + searchName + "%' OR customer.lastname LIKE '%" + searchName + "%'";
                    using (IDbCommand command = MySQLDAOFactory.GetDatabase().CreateCommand(query, connection))
                    {
                        using (IDataReader reader = command.ExecuteReader())
                        {
                            List<Customer> customerList = new List<Customer>();
                            while (reader.Read())
                            {
                                Customer c = new Customer(
                                    reader["firstname"].ToString(),
                                    reader["lastname"].ToString(),
                                    reader["phonenumber"].ToString(),
                                    reader["street"].ToString(),
                                    reader["housenumber"].ToString(),
                                    reader["town"].ToString()
                                    );
                                customerList.Add(c);
                            }
                            customers = customerList.ToArray();
                            if (customers.Length > 0)
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

        public bool DeleteCustomer(Customer customer)
        {
            if (customer.internalID.Equals(-1))
            {
                throw new Exception("Customer to remove had no ID!");
            }

            try
            {
                using (IDbConnection connection = MySQLDAOFactory.GetDatabase().CreateOpenConnection())
                {
                    string query = "DELETE FROM customer WHERE customer.idcustomer = '" + customer.internalID.ToString() + "'";
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
