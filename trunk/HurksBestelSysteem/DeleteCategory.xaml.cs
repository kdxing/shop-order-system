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
    /// Interaction logic for DeleteCategory.xaml
    /// </summary>
    public partial class DeleteCategory : Window
    {
        private ProductCategory[] totalCategories;
        private DataAccess access;

        public DeleteCategory()
        {
            InitializeComponent();
            access = new DataAccess();
            GetAvailableCategories();
        }

        private void UpdateCategoryLists()
        {
            lbAvailableCategories.Items.Clear();
            for (int i = 0; i < totalCategories.Length; i++)
            {
                lbAvailableCategories.Items.Add(totalCategories[i]);
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

        private void btnDeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            object selectedObject = lbAvailableCategories.SelectedItem;
            if (selectedObject == null)
            {
                MessageBox.Show("Selecteer een categorie om te verwijderen.", "Selectie", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            ProductCategory cat = (ProductCategory)selectedObject;
            MessageBoxResult result = MessageBox.Show("Weet U zeker dat u categorie '" + cat.name + "' wilt verwijderen?", "Bevestiging", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                if (access.RemoveCategory(cat))
                {
                    MessageBox.Show("Categorie verwijderd!", "Verwijderd", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Categorie kon niet verwijderd worden!", "Mislukt", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Verwijderding afgebroken", "Afgebroken", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            GetAvailableCategories();
        }
    }
}
