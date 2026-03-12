using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CliningCompany.Connect;

namespace CliningCompany.Pages
{
    public partial class AddReviewPage : Page
    {
        private int _orderId;

        public AddReviewPage(int orderId)
        {
            InitializeComponent();
            _orderId = orderId;
            cmbRating.SelectedIndex = 4; 
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbRating.SelectedItem == null)
            {
                MessageBox.Show("Выберите оценку!");
                return;
            }

            int rating = int.Parse((cmbRating.SelectedItem as ComboBoxItem).Content.ToString());
            string comment = txtComment.Text.Trim();

            var existing = Connection.entities.Reviews.FirstOrDefault(r => r.OrderId == _orderId);
            if (existing != null)
            {
                MessageBox.Show("Вы уже оставили отзыв на этот заказ.");
                NavigationService.GoBack();
                return;
            }

            var review = new Reviews
            {
                OrderId = _orderId,
                Rating = rating,
                Comment = comment,
                ReviewDate = DateTime.Now
            };
            Connection.entities.Reviews.Add(review);
            Connection.entities.SaveChanges();

            MessageBox.Show("Спасибо за отзыв!");
            NavigationService.GoBack();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}