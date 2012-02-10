using System;
using HurksBestelSysteem.Domain;

namespace HurksBestelSysteem.DAO
{
    public interface OrderDAO
    {
        bool AddOrder(Order order);
        bool RemoveOrder(Order order);
        bool GetOrdersByCustomer(Customer customer, out Order[] orders);
        bool GetOrdersByPickupDate(DateTime date, out Order[] orders);
        bool GetOrdersByOrderedDate(DateTime date, out Order[] orders);
        bool GetOrdersByEmployee(Employee employee, out Order[] orders);
    }
}

