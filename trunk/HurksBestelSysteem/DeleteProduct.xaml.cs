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
    public partial class DeleteProduct : Window
    {
        DataAccess access;

        public DeleteProduct()
        {
            InitializeComponent();
            access = new DataAccess();
        }

        private void tbSearchInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetSearchResult();
        }

        private void GetSearchResult()
        {
            if (tbSearchInput.Text.Equals(""))
            {
                lbSearchResult.Items.Clear();
                return;
            }

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

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lbSearchResult.SelectedItems.Count > 1)
            {
                MessageBox.Show(this, "U kunt niet meerdere producten tegelijk verwijderen!", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                object selected = lbSearchResult.SelectedItem;
                if (selected == null)
                {
                    MessageBox.Show(this, "Geen product geselecteerd!", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    Product p = (Product)selected;
                    MessageBoxResult confirmation = MessageBox.Show(this, "Weet u zeker dat u product '" + p.productName + "' (" + p.productCode + ") wilt verwijderen?", "Verwijderbevestiging", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (confirmation == MessageBoxResult.Yes)
                    {
                        if (access.RemoveProduct(p))
                        {
                            MessageBox.Show(this, "Product verwijderd!", "Geslaagd", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show(this, "Product kon niet verwijderd worden!", "Mislukt", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        //regardless of the outcome, update our productlist, so the user can visually see what happened to the products
                        GetSearchResult();
                    }
                    else
                    {
                        MessageBox.Show(this, "Verwijdering afgebroken!", "Afgebroken", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            //update out list, in case an interaction with another window (delete window, add window, etc) changed the database
            GetSearchResult();
        }
    }
}
