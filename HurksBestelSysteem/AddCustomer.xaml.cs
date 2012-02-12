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
    /// Interaction logic for AddCustomer.xaml
    /// </summary>
    public partial class AddCustomer : Window
    {
        private DataAccess access;

        public AddCustomer()
        {
            InitializeComponent();
            access = new DataAccess();
        }

        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            string firstName = tbFirstName.Text;
            string lastName = tbLastName.Text;
            string phoneNumber = tbPhoneNumber.Text;
            string street = tbStreet.Text;
            string streetNumber = tbHouseNumber.Text;
            string town = tbTown.Text;

            if (lastName.Equals(""))
            {
                MessageBox.Show(this, "Een achternaam is minimaal verplicht!", "Te weinig gegevens", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Customer c = new Customer(firstName, lastName, phoneNumber, street, streetNumber, town);
            if (access.AddCustomer(c))
            {
                MessageBox.Show(this, "Klant succesvol toegevoegd!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(this, "Klant kon niet toegevoegd worden!", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
