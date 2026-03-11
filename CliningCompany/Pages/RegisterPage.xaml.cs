using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CliningCompany.Connect;

namespace CliningCompany.Pages
{
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();
            string confirm = txtConfirmPassword.Password.Trim();

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirm))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            if (password != confirm)
            {
                MessageBox.Show("Пароли не совпадают!");
                return;
            }

            if (Connection.entities.Users.Any(u => u.Username == username))
            {
                MessageBox.Show("Пользователь с таким логином уже существует!");
                return;
            }

            var newUser = new Users
            {
                FullName = fullName,
                Username = username,
                Password = password,
                Role = "user"
            };
            Connection.entities.Users.Add(newUser);
            Connection.entities.SaveChanges();

            MessageBox.Show("Регистрация успешна! Теперь войдите.");
            NavigationService.Navigate(new LoginPage());
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }
    }
}