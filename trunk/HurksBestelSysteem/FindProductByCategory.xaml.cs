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
    public partial class FindProductByCategory : Window
    {
        private DataAccess access;
        private ProductCategory[] totalCategories;
        private List<ProductCategory> chosenCategories;
        private List<ProductCategory> availableCategories;
        private CategoryComparator categoryComparator;

        public FindProductByCategory()
        {
            InitializeComponent();
            access = new DataAccess();
            totalCategories = null;
            chosenCategories = new List<ProductCategory>();
            availableCategories = new List<ProductCategory>();
            categoryComparator = new CategoryComparator();
            GetAvailableCategories();
        }

        private void GetSearchResult()
        {
            lbSearchResult.Items.Clear();
            if (chosenCategories.Count > 0)
            {
                Product[] products;
                access.GetProductsByCategory(chosenCategories.ToArray(), out products);
                for (int i = 0; i < products.Length; i++)
                {
                    lbSearchResult.Items.Add(products[i]);
                }
            }
        }

        private void lbSearchResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object selected = lbSearchResult.SelectedItem;
            if (selected == null)
            {
                //clear GUI
            }
            else
            {
                Product p = (Product)selected;
                //display in GUI
            }
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
                GetSearchResult();
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
                GetSearchResult();
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

        private void Window_Activated(object sender, EventArgs e)
        {
            //interaction in another window (category delete/add window) could've caused some categories to be removed or added
            //so we need to update our category lists to prevent errors
            GetAvailableCategories();
        }
    }
}
