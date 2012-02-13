using System;
using System.Windows;
using System.Windows.Controls;
using HurksBestelSysteem.Domain;

namespace HurksBestelSysteem
{
    /// <summary>
    /// Interaction logic for FindOrder.xaml
    /// </summary>
    public partial class FindOrder : Window
    {
        private DataAccess access;

        public FindOrder()
        {
            InitializeComponent();
            access = new DataAccess();
            GetSearchResult();
        }

        private void GetSearchResult()
        {
            Order[] orders;
            lbOrdersList.Items.Clear();
            string input = tbSearchInput.Text;
            if (input.Equals(""))
            {
                access.GetAllOrders(out orders);
            }
            else
            {
                orders = new Order[0];
                //access.GetCustomersByName(input, out orders);
            }
            for (int i = 0; i < orders.Length; i++)
            {
                lbOrdersList.Items.Add(orders[i]);
            }
        }

        private void tbSearchInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetSearchResult();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            GetSearchResult();
        }
    }
}
