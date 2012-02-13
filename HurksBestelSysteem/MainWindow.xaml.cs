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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataAccess dataAccess;

        public MainWindow()
        {
            InitializeComponent();
            dataAccess = new DataAccess();
        }

        private void btnFindProduct_Click(object sender, RoutedEventArgs e)
        {
            FindProduct p = new FindProduct();
            p.Owner = this;
            p.Show();
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            AddProduct a = new AddProduct();
            a.Owner = this;
            a.Show();
        }

        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            DeleteProduct d = new DeleteProduct();
            d.Owner = this;
            d.Show();
        }

        private void btnFindProductsByCategory_Click(object sender, RoutedEventArgs e)
        {
            FindProductByCategory c = new FindProductByCategory();
            c.Owner = this;
            c.Show();
        }

        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            AddCategory a = new AddCategory();
            a.Owner = this;
            a.Show();
        }

        private void btnDeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            DeleteCategory d = new DeleteCategory();
            d.Owner = this;
            d.Show();
        }

        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            AddCustomer a = new AddCustomer();
            a.Owner = this;
            a.Show();
        }

        private void btnAddBestelling_Click(object sender, RoutedEventArgs e)
        {
            AddOrder o = new AddOrder();
            o.Owner = this;
            o.Show();
        }

        private void btnSearchOrder_Click(object sender, RoutedEventArgs e)
        {
            FindOrder f = new FindOrder();
            f.Owner = this;
            f.Show();
        }
    }
}
