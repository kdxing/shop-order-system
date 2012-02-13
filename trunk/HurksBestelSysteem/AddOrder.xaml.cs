using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HurksBestelSysteem.Domain;

namespace HurksBestelSysteem
{
    /// <summary>
    /// Interaction logic for AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Window
    {
        private DataAccess access;
        private int timeHours = 12;
        private int timeMinutes = 0;

        public AddOrder()
        {
            InitializeComponent();
            access = new DataAccess();
            GetSearchResult();
        }

        private void GetSearchResult()
        {
            Customer[] customers;
            lbSelectCustomerList.Items.Clear();
            string input = tbSearchInput.Text;
            if (input.Equals(""))
            {
                access.GetAllCustomers(out customers);
            }
            else
            {
                access.GetCustomersByName(input, out customers);
            }
            for (int i = 0; i < customers.Length; i++)
            {
                lbSelectCustomerList.Items.Add(customers[i]);
            }
        }

        #region TIME_CONTROL
        private void tbHours_TextChanged(object sender, TextChangedEventArgs e)
        {
            int newHours;
            if (Int32.TryParse(tbHours.Text, out newHours))
            {
                if (newHours >= 0 && newHours <= 23)
                {
                    timeHours = newHours;
                }
            }
            SetHours(timeHours);
        }

        private void tbMinutes_TextChanged(object sender, TextChangedEventArgs e)
        {
            int newMinutes;
            if (Int32.TryParse(tbMinutes.Text, out newMinutes))
            {
                if (newMinutes >= 0 && newMinutes <= 59)
                {
                    timeMinutes = newMinutes;
                }
            }
            SetMinutes(timeMinutes);
        }

        private void sbHours_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int difference = 0;
            double scrolled = e.NewValue - e.OldValue;
            if (scrolled > 0)
            {
                difference = -1;
            }
            else if (scrolled < 0)
            {
                difference = 1;
            }
            else
            {
                return;
            }
            if (tbHours == null) { return; }
            int newHours = timeHours + difference;
            if (newHours >= 0 && newHours <= 23)
            {
                timeHours = newHours;
            }
            SetHours(timeHours);
        }

        private void sbMinutes_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int difference = 0;
            double scrolled = e.NewValue - e.OldValue;
            if (scrolled > 0)
            {
                difference = -1;
            }
            else if (scrolled < 0)
            {
                difference = 1;
            }
            else
            {
                return;
            }
            if (tbMinutes == null) { return; }
            int newMinutes = timeMinutes + difference;
            if (newMinutes >= 0 && newMinutes <= 59)
            {
                timeMinutes = newMinutes;
            }
            SetMinutes(timeMinutes);
        }

        private void SetMinutes(int newMinutes)
        {
            tbMinutes.Text = newMinutes.ToString();
        }

        private void SetHours(int newHours)
        {
            tbHours.Text = newHours.ToString();
        }
        #endregion TIME_CONTROL

        private void tbSearchInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetSearchResult();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            GetSearchResult();
        }

        private void btnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            object selectedObject = lbSelectCustomerList.SelectedItem;
            if (selectedObject == null)
            {
                MessageBox.Show(this, "Geen klant geselecteerd!", "Onvoldoende gegevens", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Customer customer = (Customer)selectedObject;
            DateTime dateTimeOrdered = DateTime.Now;
            DateTime dateTimePickup = dpPickupDate.SelectedDate.GetValueOrDefault(DateTime.Now);
            dateTimePickup = dateTimePickup.AddHours(timeHours);
            dateTimePickup = dateTimePickup.AddMinutes(timeMinutes);
            Employee employee = new Employee("iedereen", 1);
            string description = tbDescription.Text;

            Order order = new Order(customer, dateTimeOrdered, dateTimePickup, employee, description);
            if (access.AddOrder(order))
            {
                MessageBox.Show(this, "Order succesvol toegevoegd!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(this, "Order kon niet toegevoegd worden!", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
