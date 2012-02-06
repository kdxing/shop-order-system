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
    /// Interaction logic for FindProduct.xaml
    /// </summary>
    public partial class FindProduct : Window
    {
        DataAccess access;

        public FindProduct()
        {
            InitializeComponent();
            access = new DataAccess();
        }

        private void tbSearchInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            Product[] products;
            access.GetProductsByName(tbSearchInput.Text, out products);
            lbSearchResult.Items.Clear();
            for (int i = 0; i < products.Length; i++)
            {
                lbSearchResult.Items.Add(products[i]);
            }
        }

        private void lbSearchResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
