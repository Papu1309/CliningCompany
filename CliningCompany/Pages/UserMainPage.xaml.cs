using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CliningCompany.Connect;
using System.Data.Entity; // для Include

namespace CliningCompany.Pages
{
    public partial class UserMainPage : Page
    {
        public UserMainPage()
        {
            InitializeComponent();
            LoadServices();
        }

        private void LoadServices(string filter = "")
        {
            var query = Connection.entities.Services.Include(s => s.Categories).AsQueryable();
            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(s => s.Name.Contains(filter));
            }
            lvServices.ItemsSource = query.ToList();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadServices(txtSearch.Text);
        }

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            var selected = lvServices.SelectedItem as Services;
            if (selected == null)
            {
                MessageBox.Show("Выберите услугу!");
                return;
            }
            NavigationService.Navigate(new PaymentPage(selected));
        }

        private void btnMyOrders_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UserOrdersPage());
        }

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UserProfilePage());
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            AppState.CurrentUser = null;
            NavigationService.Navigate(new LoginPage());
        }
    }
}