using System;
namespace HurksBestelSysteem.Domain
{
    public sealed class Order
    {
        public Order_Entry[] order_entries;
        public Customer customer;
        public DateTime dateTimeOrdered;
        public DateTime dateTimePickup;
        public Employee employee;
        public string description;
        public int internalID;

        public Order(Customer customer, DateTime dateTimeOrdered, DateTime dateTimePickup, Employee employee, string description)
        {
            this.customer = customer;
            this.dateTimeOrdered = dateTimeOrdered;
            this.dateTimePickup = dateTimePickup;
            this.employee = employee;
            this.description = description;
            this.internalID = -1;
        }

        public Order(Customer customer, DateTime dateTimeOrdered, DateTime dateTimePickup, Employee employee, string description, Order_Entry[] order_entries)
        {
            this.customer = customer;
            this.dateTimeOrdered = dateTimeOrdered;
            this.dateTimePickup = dateTimePickup;
            this.employee = employee;
            this.description = description;
            this.order_entries = order_entries;
            this.internalID = -1;
        }

        public Order(Customer customer, DateTime dateTimeOrdered, DateTime dateTimePickup, Employee employee, string description, int internalID)
        {
            this.customer = customer;
            this.dateTimeOrdered = dateTimeOrdered;
            this.dateTimePickup = dateTimePickup;
            this.employee = employee;
            this.description = description;
            this.internalID = internalID;
        }

        public Order(Customer customer, DateTime dateTimeOrdered, DateTime dateTimePickup, Employee employee, string description, Order_Entry[] order_entries, int internalID)
        {
            this.customer = customer;
            this.dateTimeOrdered = dateTimeOrdered;
            this.dateTimePickup = dateTimePickup;
            this.employee = employee;
            this.description = description;
            this.order_entries = order_entries;
            this.internalID = internalID;
        }

        public override string ToString()
        {
            return customer.lastName + " (" + customer.firstName + ")" + dateTimePickup.ToString();
        }
    }
}
