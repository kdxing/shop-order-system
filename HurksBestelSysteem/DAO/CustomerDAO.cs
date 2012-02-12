using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HurksBestelSysteem.Domain;

namespace HurksBestelSysteem.DAO
{
    public interface CustomerDAO
    {
        bool AddCustomer(Customer customer);
        bool DeleteCustomer(Customer customer);
        bool UpdateCustomer(Customer customer);
        bool GetAllCustomers(out Customer[] customers);
        bool GetCustomersByName(string customerName, out Customer[] customers);
    }
}
