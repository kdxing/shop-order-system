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
    }
}
