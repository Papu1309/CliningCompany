using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using CliningCompany.Connect;

namespace CliningCompany.Pages
{
    public partial class PaymentPage : Page
    {
        private Services _selectedService;

        public PaymentPage(Services service)
        {
            InitializeComponent();
            _selectedService = service;
            txtServiceName.Text = service.Name;
            txtServicePrice.Text = service.Price.ToString("C");

            rbCard.Checked += (s, e) => CardDetailsPanel.Visibility = Visibility.Visible;
            rbCash.Checked += (s, e) => CardDetailsPanel.Visibility = Visibility.Collapsed;
        }

        private void btnPay_Click(object sender, RoutedEventArgs e)
        {
            string paymentMethod = rbCard.IsChecked == true ? "card" : "cash";

            if (paymentMethod == "card")
            {
                string cardNumber = txtCardNumber.Text.Replace(" ", "").Replace("-", "");
                string expiry = txtExpiry.Text.Trim();
                string cvv = txtCVV.Password.Trim();

                if (cardNumber.Length > 16)
                {
                    MessageBox.Show("Номер карты не может быть больше 16 цифр!");
                    return;
                }
                if (!Regex.IsMatch(cardNumber, @"^\d{16}$"))
                {
                    MessageBox.Show("Номер карты должен содержать 16 цифр!");
                    return;
                }
                if (!Regex.IsMatch(expiry, @"^(0[1-9]|1[0-2])\/\d{2}$"))
                {
                    MessageBox.Show("Срок в формате ММ/ГГ (например, 12/25)!");
                    return;
                }
                if (!Regex.IsMatch(cvv, @"^\d{3}$"))
                {
                    MessageBox.Show("CVV должен содержать 3 цифры!");
                    return;
                }
            }

            var order = new Orders
            {
                UserId = AppState.CurrentUser.Id,
                ServiceId = _selectedService.Id,
                OrderDate = DateTime.Now,
                PaymentMethod = paymentMethod,
                Status = "paid"
            };

            order.PickupAddress = "Просьба позвонить по номеру +79391234567 для согласования место встречи с бригадой";
            DateTime pickup = DateTime.Now.AddHours(2);
            if (pickup.Hour < 9) pickup = pickup.Date.AddHours(9);
            if (pickup.Hour > 20) pickup = pickup.Date.AddDays(1).AddHours(9);
            order.PickupTime = pickup;

            Connection.entities.Orders.Add(order);
            Connection.entities.SaveChanges();

            MessageBox.Show($"Заказ оформлен!\nАдрес : {order.PickupAddress}\nВремя: {order.PickupTime:dd.MM.yyyy HH:mm}",
                            "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

            NavigationService.Navigate(new UserMainPage());
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UserMainPage());
        }
    }
}