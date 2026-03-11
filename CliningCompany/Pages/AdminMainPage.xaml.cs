using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CliningCompany.Pages;

namespace CliningCompany.Pages
{
    public partial class AdminMainPage : Page
    {
        public class MenuItem
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public Type PageType { get; set; }
        }

        public AdminMainPage()
        {
            InitializeComponent();
            LoadMenu();
        }

        private void LoadMenu()
        {
            var items = new List<MenuItem>
            {
                new MenuItem { Title = "Управление услугами", Description = "Добавление, редактирование, удаление услуг", PageType = typeof(AdminServicesPage) },
                new MenuItem { Title = "Просмотр заказов", Description = "Фильтрация и изменение статусов заказов", PageType = typeof(AdminOrdersPage) },
                new MenuItem { Title = "Статистика", Description = "Количество заказов, выручка", PageType = typeof(AdminStatisticsPage) }
            };
            lvMenu.ItemsSource = items;
            lvMenu.SelectionChanged += LvMenu_SelectionChanged;
        }

        private void LvMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvMenu.SelectedItem is MenuItem item)
            {
                NavigationService.Navigate(Activator.CreateInstance(item.PageType) as Page);
                lvMenu.SelectedItem = null;
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            AppState.CurrentUser = null;
            NavigationService.Navigate(new LoginPage());
        }
    }
}