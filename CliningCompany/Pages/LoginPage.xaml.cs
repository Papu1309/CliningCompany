using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CliningCompany.Connect;

namespace CliningCompany.Pages
{
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            var user = Connection.entities.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль!");
                return;
            }

            AppState.CurrentUser = user;

            if (user.Role == "admin")
                NavigationService.Navigate(new AdminMainPage());
            else
                NavigationService.Navigate(new UserMainPage());
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegisterPage());
        }
    }
}