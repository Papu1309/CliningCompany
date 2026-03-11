using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CliningCompany.Connect;
using System.Data.Entity;

namespace CliningCompany.Pages
{
    public partial class AdminStatisticsPage : Page
    {
        public AdminStatisticsPage()
        {
            InitializeComponent();
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            var orders = Connection.entities.Orders.Include(o => o.Services).ToList();

            int total = orders.Count;
            int completed = orders.Count(o => o.Status == "completed");
            int cancelled = orders.Count(o => o.Status == "cancelled");
            decimal revenue = orders.Where(o => o.Status == "completed").Sum(o => o.Services.Price);

            txtTotalOrders.Text = total.ToString();
            txtCompletedOrders.Text = completed.ToString();
            txtCancelledOrders.Text = cancelled.ToString();
            txtTotalRevenue.Text = revenue.ToString("C");
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdminMainPage());
        }
    }
}