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
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        private DataAccess access;

        public AddProduct()
        {
            InitializeComponent();
            access = new DataAccess();
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            string productName = tbProductName.Text;
            if (productName.Equals("") == true)
            {
                MessageBox.Show(this, "U moet een geldige productnaam ingeven!", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int productCode = -1;
            string productCodeText = tbProductCode.Text;
            if (productCodeText.Equals("") == true)
            {
                MessageBox.Show(this, "U moet een geldige productcode ingeven!", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                try
                {
                    productCode = Convert.ToInt32(productCodeText);
                }
                catch
                {
                    MessageBox.Show(this, "U moet een geldige productcode ingeven!", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            string description = tbDescription.Text; //no checking needed, as is optional

            decimal price = -1;
            string priceAsText = tbPrice.Text;
            if (priceAsText.Equals("") == true)
            {
                MessageBox.Show(this, "U moet een geldige prijs opgeven!", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                try
                {
                    price = Decimal.Parse(priceAsText, System.Globalization.NumberStyles.Currency, System.Globalization.NumberFormatInfo.CurrentInfo);
                }
                catch
                {
                    MessageBox.Show(this, "U moet een geldige prijs opgeven!", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            Product.PriceType priceType = Product.PriceType.WEIGHT;
            if (rbPriceTypeUnit.IsChecked == true)
            { priceType = Product.PriceType.UNIT; }
            else if (rbPriceTypeWeight.IsChecked == true)
            { priceType = Product.PriceType.WEIGHT; }
            else
            {
                MessageBox.Show(this, "U heeft geen prijstype ingesteld!", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Product p = new Product(
                productName,
                productCode,
                description,
                price,
                priceType
                );
            access.AddProduct(p);
            MessageBox.Show(this, "Product succesvol toegevoegd!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
