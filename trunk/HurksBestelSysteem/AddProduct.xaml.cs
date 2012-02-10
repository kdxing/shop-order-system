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
        private ProductCategory[] totalCategories;
        private List<ProductCategory> chosenCategories;
        private List<ProductCategory> availableCategories;
        private CategoryComparator categoryComparator;

        public AddProduct()
        {
            InitializeComponent();
            access = new DataAccess();
            totalCategories = null;
            chosenCategories = new List<ProductCategory>();
            availableCategories = new List<ProductCategory>();
            categoryComparator = new CategoryComparator();
            GetAvailableCategories();
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
                priceType,
                chosenCategories.ToArray()
                );
            if (access.AddProduct(p))
            {
                MessageBox.Show(this, "Product succesvol toegevoegd!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(this, "Product kon niet toegevoegd worden!", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateCategoryLists()
        {
            //check if chosen items still exist in the latest retrieved category list
            for (int i = 0; i < chosenCategories.Count; i++)
            {
                ProductCategory c = chosenCategories[i];
                if (totalCategories.Contains<ProductCategory>(c, categoryComparator) == false) //DIT KAN FOUT GAAN MSS, MAAK COMPARATOR OID
                {
                    //if they do not, remove them
                    chosenCategories.Remove(c);
                }
            }
            //recompile our available list
            availableCategories.Clear();
            for (int i = 0; i < totalCategories.Length; i++)
            {
                ProductCategory c = totalCategories[i];
                if (chosenCategories.Contains(c, categoryComparator) == false) //DIT KAN FOUT GAAN MSS, MAAK COMPARATOR OID
                {
                    availableCategories.Add(c);
                }
            }
            //now update our GUI
            lbAvailableCategories.Items.Clear();
            for (int i = 0; i < availableCategories.Count; i++)
            {
                lbAvailableCategories.Items.Add(availableCategories[i]);
            }
            lbChosenCategories.Items.Clear();
            for (int i = 0; i < chosenCategories.Count; i++)
            {
                lbChosenCategories.Items.Add(chosenCategories[i]);
            }
        }

        private void GetAvailableCategories()
        {
            //we're getting the latest categories from the database
            //so clear our existing categories
            ProductCategory[] categories;
            access.GetProductCategories(out categories);
            totalCategories = categories;
            //update our available and chosen lists and the GUI
            UpdateCategoryLists(); 
        }

        private void btnAddCategories_Click(object sender, RoutedEventArgs e)
        {
            if (lbAvailableCategories.SelectedItems.Count > 0)
            {
                for (int i = 0; i < lbAvailableCategories.SelectedItems.Count; i++)
                {
                    ProductCategory c = (ProductCategory)lbAvailableCategories.SelectedItems[i];
                    availableCategories.Remove(c);
                    chosenCategories.Add(c);
                }
                UpdateCategoryLists();
            }
        }

        private void btnDeleteCategories_Click(object sender, RoutedEventArgs e)
        {
            if (lbChosenCategories.SelectedItems.Count > 0)
            {
                for (int i = 0; i < lbChosenCategories.SelectedItems.Count; i++)
                {
                    ProductCategory c = (ProductCategory)lbChosenCategories.SelectedItems[i];
                    chosenCategories.Remove(c);
                    availableCategories.Add(c);
                }
                UpdateCategoryLists();
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            //interaction in another window (category delete/add window) could've caused some categories to be removed or added
            //so we need to update our category lists to prevent errors
            GetAvailableCategories();
        }
    }
}
