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
    /// Interaction logic for AddCategory.xaml
    /// </summary>
    public partial class AddCategory : Window
    {
        private DataAccess access;

        public AddCategory()
        {
            InitializeComponent();
            access = new DataAccess();
        }

        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            string name = tbCategoryName.Text;
            string description = tbCategoryDescription.Text;
            if (name.Equals(""))
            {
                MessageBox.Show(this, "U moet een naam voor de categorie invullen!", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ProductCategory c = new ProductCategory(name, description);
            if (access.AddCategory(c))
            {
                MessageBox.Show(this, "Categorie succesvol toegevoegd!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(this, "Categorie kon niet toegevoegd worden!", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
