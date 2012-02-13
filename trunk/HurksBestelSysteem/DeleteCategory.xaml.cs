using System.Windows;
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
                MessageBox.Show(this, "Selecteer een categorie om te verwijderen.", "Selectie", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            ProductCategory cat = (ProductCategory)selectedObject;
            MessageBoxResult result = MessageBox.Show(this, "Weet U zeker dat u categorie '" + cat.name + "' wilt verwijderen?", "Bevestiging", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                if (access.RemoveCategory(cat))
                {
                    MessageBox.Show(this, "Categorie verwijderd!", "Verwijderd", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(this, "Categorie kon niet verwijderd worden!", "Mislukt", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show(this, "Verwijderding afgebroken", "Afgebroken", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            GetAvailableCategories();
        }
    }
}
