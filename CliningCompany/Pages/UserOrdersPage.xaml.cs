using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CliningCompany.Connect;
using System.Data.Entity;

namespace CliningCompany.Pages
{
    public partial class UserOrdersPage : Page
    {
        public UserOrdersPage()
        {
            InitializeComponent();
            this.Loaded += UserOrdersPage_Loaded;
        }

        private void UserOrdersPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadOrders();
        }

        private void LoadOrders()
        {
            try
            {
                if (lvOrders == null)
                {
                    Dispatcher.BeginInvoke(new Action(() => LoadOrders()));
                    return;
                }

                var orders = Connection.entities.Orders
                    .Include("Services")
                    .Where(o => o.UserId == AppState.CurrentUser.Id)
                    .ToList();

                orders = orders.OrderByDescending(o => o.OrderDate).ToList();
                lvOrders.ItemsSource = orders;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки заказов: " + ex.Message);
            }
        }

        private void CancelOrder_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int orderId = (int)btn.Tag;
            var order = Connection.entities.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null) return;

            if (order.Status != "paid")
            {
                MessageBox.Show("Этот заказ уже нельзя отменить.");
                return;
            }

            if (MessageBox.Show("Отменить заказ?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                order.Status = "cancelled";
                Connection.entities.SaveChanges();
                LoadOrders();
            }
        }

        private void AddReview_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int orderId = (int)btn.Tag;
            NavigationService.Navigate(new AddReviewPage(orderId));
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UserMainPage());
        }
    }
}