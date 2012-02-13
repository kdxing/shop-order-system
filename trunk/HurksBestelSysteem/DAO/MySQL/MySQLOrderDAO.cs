using System;
using System.Collections.Generic;
using HurksBestelSysteem.Domain;
using System.Data;
using MySql.Data.MySqlClient;
using HurksBestelSysteem.Database;
using System.Text;

namespace HurksBestelSysteem.DAO.MySQL
{
    public class MySQLOrderDAO : OrderDAO
    {
        private const string dateFormatReading = "d-M-yyyy H:mm:ss";
        private const string dateFormatWriting = "yyyy-MM-dd HH:mm:ss";

        public bool AddOrder(Order order)
        {
            if (order.internalID.Equals(-1) == false)
            {
                throw new Exception("Order to insert into database already has database id!");
            }
            if (order.customer.internalID.Equals(-1) == true)
            {
                throw new Exception("Order to insert into database didn't have valid customer!");
            }
            if (order.employee.internalID.Equals(-1) == true)
            {
                throw new Exception("Order to insert into database didn't have valid employee!");
            }

            using (IDbConnection connection = MySQLDAOFactory.GetDatabase().CreateOpenConnection())
            {
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = "INSERT INTO " + MySQLDAOFactory.DatabaseName + ".order (customerid, ordered_datetime, pickup_datetime, employee_id, description) VALUES "
                        + "('" + order.customer.internalID.ToString() + "', '" + order.dateTimeOrdered.ToString(dateFormatWriting) + "', '" + order.dateTimePickup.ToString(dateFormatWriting) + "', '" + order.employee.internalID.ToString() + "', '" + order.description + "')";
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
                                    order.internalID = Convert.ToInt32(reader[0]);
                                }
                            }
                        }
                        if (order.internalID.Equals(-1))
                        {
                            transaction.Rollback();
                            throw new Exception("Order inserted succesfully, but could not retrieve ID afterwards. Rollback.");
                        }
                        if (order.order_entries != null && order.order_entries.Length > 0)
                        {
                            StringBuilder catQuery = new StringBuilder("INSERT INTO order_entry (orderid, productid, quantity) VALUES ", order.order_entries.Length * 2);
                            for (int i = 0; i < order.order_entries.Length; i++)
                            {
                                catQuery.Append("(" + order.internalID + ", " + order.order_entries[i].product.internalID + ", " + order.order_entries[i].quantity + ")");
                                if (i != (order.order_entries.Length - 1))
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

        public bool GetAllOrders(out Order[] orders)
        {
            try
            {
                using (IDbConnection connection = MySQLDAOFactory.GetDatabase().CreateOpenConnection())
                {
                    string query = "SELECT * FROM " + MySQLDAOFactory.DatabaseName + ".order";
                    using (IDbCommand command = MySQLDAOFactory.GetDatabase().CreateCommand(query, connection))
                    {
                        using (IDataReader reader = command.ExecuteReader())
                        {
                            List<Order> ordersList = new List<Order>();
                            while (reader.Read())
                            {
                                DateTime ordered_datetime;
                                DateTime pickup_datetime;
                                if (DateTime.TryParseExact(reader["ordered_datetime"].ToString(), dateFormatReading, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.AssumeLocal, out ordered_datetime) &&
                                    DateTime.TryParseExact(reader["pickup_datetime"].ToString(), dateFormatReading, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.AssumeLocal, out pickup_datetime))
                                {
                                    Order o = new Order(
                                        new Customer("", "", "", "", "", "", Convert.ToInt32(reader["customerid"])),
                                        ordered_datetime,
                                        pickup_datetime,
                                        new Employee("", Convert.ToInt32(reader["employee_id"])),
                                        reader["description"].ToString(),
                                        Convert.ToInt32(reader["idorder"])
                                        );
                                    ordersList.Add(o);
                                }
                                else
                                {
                                    throw new Exception("Couldn't cast to datetime!");
                                }
                            }
                            orders = ordersList.ToArray();
                            if (orders.Length > 0)
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

        public bool GetOrdersByPickupDate(DateTime date, out Order[] orders)
        {
            try
            {
                using (IDbConnection connection = MySQLDAOFactory.GetDatabase().CreateOpenConnection())
                {
                    string query = "SELECT * FROM " + MySQLDAOFactory.DatabaseName + ".orderr WHERE order.pickup_datetime = '" + date.Date.ToString("format") + "'";
                    using (IDbCommand command = MySQLDAOFactory.GetDatabase().CreateCommand(query, connection))
                    {
                        using (IDataReader reader = command.ExecuteReader())
                        {
                            List<Order> ordersList = new List<Order>();
                            while (reader.Read())
                            {
                                DateTime ordered_datetime;
                                DateTime pickup_datetime;
                                if (DateTime.TryParseExact(reader["ordered_datetime"].ToString(), dateFormatReading, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.AssumeLocal, out ordered_datetime) &&
                                    DateTime.TryParseExact(reader["pickup_datetime"].ToString(), dateFormatReading, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.AssumeLocal, out pickup_datetime))
                                {
                                    Order o = new Order(
                                        new Customer("", "", "", "", "", "", Convert.ToInt32(reader["customerid"])),
                                        ordered_datetime,
                                        pickup_datetime,
                                        new Employee("", Convert.ToInt32(reader["employee_id"])),
                                        reader["description"].ToString(),
                                        Convert.ToInt32(reader["idorder"])
                                        );
                                    ordersList.Add(o);
                                }
                                else
                                {
                                    throw new Exception("Couldn't cast to datetime!");
                                }
                            }
                            orders = ordersList.ToArray();
                            if (orders.Length > 0)
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

        public bool GetOrdersByOrderedDate(DateTime date, out Order[] orders)
        {
            try
            {
                using (IDbConnection connection = MySQLDAOFactory.GetDatabase().CreateOpenConnection())
                {
                    string query = "SELECT * FROM " + MySQLDAOFactory.DatabaseName + ".order WHERE order.ordered_datetime = '" + date.Date.ToString("format") + "'";
                    using (IDbCommand command = MySQLDAOFactory.GetDatabase().CreateCommand(query, connection))
                    {
                        using (IDataReader reader = command.ExecuteReader())
                        {
                            List<Order> ordersList = new List<Order>();
                            while (reader.Read())
                            {
                                DateTime ordered_datetime;
                                DateTime pickup_datetime;
                                if (DateTime.TryParseExact(reader["ordered_datetime"].ToString(), dateFormatReading, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.AssumeLocal, out ordered_datetime) &&
                                    DateTime.TryParseExact(reader["pickup_datetime"].ToString(), dateFormatReading, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.AssumeLocal, out pickup_datetime))
                                {
                                    Order o = new Order(
                                        new Customer("", "", "", "", "", "", Convert.ToInt32(reader["customerid"])),
                                        ordered_datetime,
                                        pickup_datetime,
                                        new Employee("", Convert.ToInt32(reader["employee_id"])),
                                        reader["description"].ToString(),
                                        Convert.ToInt32(reader["idorder"])
                                        );
                                    ordersList.Add(o);
                                }
                                else
                                {
                                    throw new Exception("Couldn't cast to datetime!");
                                }
                            }
                            orders = ordersList.ToArray();
                            if (orders.Length > 0)
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

        public bool GetOrdersByEmployee(Employee employee, out Order[] orders)
        {
            try
            {
                using (IDbConnection connection = MySQLDAOFactory.GetDatabase().CreateOpenConnection())
                {
                    string query = "SELECT * FROM " + MySQLDAOFactory.DatabaseName + ".order WHERE order.employee_id = '" + employee.internalID.ToString() + "'";
                    using (IDbCommand command = MySQLDAOFactory.GetDatabase().CreateCommand(query, connection))
                    {
                        using (IDataReader reader = command.ExecuteReader())
                        {
                            List<Order> ordersList = new List<Order>();
                            while (reader.Read())
                            {
                                DateTime ordered_datetime;
                                DateTime pickup_datetime;
                                if (DateTime.TryParseExact(reader["ordered_datetime"].ToString(), dateFormatReading, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.AssumeLocal, out ordered_datetime) &&
                                    DateTime.TryParseExact(reader["pickup_datetime"].ToString(), dateFormatReading, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.AssumeLocal, out pickup_datetime))
                                {
                                    Order o = new Order(
                                        new Customer("", "", "", "", "", "", Convert.ToInt32(reader["customerid"])),
                                        ordered_datetime,
                                        pickup_datetime,
                                        employee,
                                        reader["description"].ToString(),
                                        Convert.ToInt32(reader["idorder"])
                                        );
                                    ordersList.Add(o);
                                }
                                else
                                {
                                    throw new Exception("Couldn't cast to datetime!");
                                }
                            }
                            orders = ordersList.ToArray();
                            if (orders.Length > 0)
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

        public bool GetOrdersByCustomer(Customer customer, out Order[] orders)
        {
            try
            {
                using (IDbConnection connection = MySQLDAOFactory.GetDatabase().CreateOpenConnection())
                {
                    string query = "SELECT * FROM " + MySQLDAOFactory.DatabaseName + ".order WHERE order.customerid = '" + customer.internalID.ToString() + "'";
                    using (IDbCommand command = MySQLDAOFactory.GetDatabase().CreateCommand(query, connection))
                    {
                        using (IDataReader reader = command.ExecuteReader())
                        {
                            List<Order> ordersList = new List<Order>();
                            while (reader.Read())
                            {
                                DateTime ordered_datetime;
                                DateTime pickup_datetime;
                                if (DateTime.TryParseExact(reader["ordered_datetime"].ToString(), dateFormatReading, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.AssumeLocal, out ordered_datetime) &&
                                    DateTime.TryParseExact(reader["pickup_datetime"].ToString(), dateFormatReading, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.AssumeLocal, out pickup_datetime))
                                {
                                    Order o = new Order(
                                        customer,
                                        ordered_datetime,
                                        pickup_datetime,
                                        new Employee("", Convert.ToInt32(reader["employee_id"])),
                                        reader["description"].ToString(),
                                        Convert.ToInt32(reader["idorder"])
                                        );
                                    ordersList.Add(o);
                                }
                                else
                                {
                                    throw new Exception("Couldn't cast to datetime!");
                                }
                            }
                            orders = ordersList.ToArray();
                            if (orders.Length > 0)
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

        public bool RemoveOrder(Order order)
        {
            if (order.internalID.Equals(-1))
            {
                throw new Exception("Order to remove had no ID!");
            }

            try
            {
                using (IDbConnection connection = MySQLDAOFactory.GetDatabase().CreateOpenConnection())
                {
                    //upon deletion, make sure both the productcode and the internal database id match, that way we're 100% sure the right product is deleted
                    string query = "DELETE FROM " + MySQLDAOFactory.DatabaseName + ".order WHERE order.idorder = '" + order.internalID.ToString() + "'";
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