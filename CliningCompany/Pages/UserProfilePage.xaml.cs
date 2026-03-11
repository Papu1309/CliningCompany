using System;
using System.Windows;
using System.Windows.Controls;
using CliningCompany.Connect;

namespace CliningCompany.Pages
{
    public partial class UserProfilePage : Page
    {
        public UserProfilePage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            var user = AppState.CurrentUser;
            txtFullName.Text = user.FullName;
            txtUsername.Text = user.Username;
        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string newPass = txtNewPassword.Password.Trim();
            string confirm = txtConfirmPassword.Password.Trim();

            if (string.IsNullOrEmpty(newPass) || string.IsNullOrEmpty(confirm))
            {
                MessageBox.Show("Заполните оба поля!");
                return;
            }

            if (newPass != confirm)
            {
                MessageBox.Show("Пароли не совпадают!");
                return;
            }

            var user = Connection.entities.Users.Find(AppState.CurrentUser.Id);
            user.Password = newPass;
            Connection.entities.SaveChanges();
            AppState.CurrentUser.Password = newPass; // обновим в памяти

            MessageBox.Show("Пароль успешно изменён!");
            txtNewPassword.Password = "";
            txtConfirmPassword.Password = "";
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UserMainPage());
        }
    }
}