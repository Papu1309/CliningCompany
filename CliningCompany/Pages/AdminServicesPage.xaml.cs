using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CliningCompany.Connect;
using System.Data.Entity;

namespace CliningCompany.Pages
{
    public partial class AdminServicesPage : Page
    {
        public AdminServicesPage()
        {
            InitializeComponent();
            LoadServices();
        }

        private void LoadServices()
        {
            lvServices.ItemsSource = Connection.entities.Services.Include(s => s.Categories).ToList();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddEditServiceWindow();
            if (dialog.ShowDialog() == true)
            {
                LoadServices();
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selected = lvServices.SelectedItem as Services;
            if (selected == null) { MessageBox.Show("Выберите услугу!"); return; }
            var dialog = new AddEditServiceWindow(selected);
            if (dialog.ShowDialog() == true)
            {
                LoadServices();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selected = lvServices.SelectedItem as Services;
            if (selected == null) { MessageBox.Show("Выберите услугу!"); return; }
            if (MessageBox.Show("Удалить услугу?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Connection.entities.Services.Remove(selected);
                Connection.entities.SaveChanges();
                LoadServices();
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdminMainPage());
        }
    }
}