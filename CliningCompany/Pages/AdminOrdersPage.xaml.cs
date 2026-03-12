using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CliningCompany.Connect;
using System.Data.Entity;

namespace CliningCompany.Pages
{
    public partial class AdminOrdersPage : Page
    {
        public AdminOrdersPage()
        {
            InitializeComponent();
            this.Loaded += AdminOrdersPage_Loaded;
        }

        private void AdminOrdersPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadOrders();
        }

        private void LoadOrders(string statusFilter = null)
        {
            try
            {
                if (lvOrders == null)
                {
                    Dispatcher.BeginInvoke(new Action(() => LoadOrders(statusFilter)));
                    return;
                }

                var orders = Connection.entities.Orders
                    .Include("Users")
                    .Include("Services")
                    .ToList();

                if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "Все")
                {
                    orders = orders.Where(o => o.Status == statusFilter).ToList();
                }

                orders = orders.OrderByDescending(o => o.OrderDate).ToList();

                lvOrders.ItemsSource = orders;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки заказов: " + ex.Message);
            }
        }

        private void cmbStatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = (cmbStatusFilter.SelectedItem as ComboBoxItem)?.Content.ToString();
            LoadOrders(selected);
        }

        private void ChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int orderId = (int)btn.Tag;
            var order = Connection.entities.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null) return;

            var dialog = new Window
            {
                Title = "Изменить статус",
                Width = 300,
                Height = 200,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Window.GetWindow(this)
            };
            var stack = new StackPanel { Margin = new Thickness(10) };
            var combo = new ComboBox { Margin = new Thickness(5) };
            combo.Items.Add("paid");
            combo.Items.Add("completed");
            combo.Items.Add("cancelled");
            combo.SelectedItem = order.Status;

            var btnSave = new Button { Content = "Сохранить", Width = 80, Margin = new Thickness(5) };
            btnSave.Click += (s, args) =>
            {
                order.Status = combo.SelectedItem.ToString();
                Connection.entities.SaveChanges();
                LoadOrders((cmbStatusFilter.SelectedItem as ComboBoxItem)?.Content.ToString());
                dialog.Close();
            };

            stack.Children.Add(new TextBlock { Text = "Выберите новый статус:", Margin = new Thickness(5) });
            stack.Children.Add(combo);
            stack.Children.Add(btnSave);
            dialog.Content = stack;
            dialog.ShowDialog();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdminMainPage());
        }
    }
}