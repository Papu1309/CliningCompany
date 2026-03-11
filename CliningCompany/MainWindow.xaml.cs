using System.Windows;
using System.Windows.Controls;
using CliningCompany.Pages;

namespace CliningCompany
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new LoginPage());
        }

        public void Navigate(Page page)
        {
            MainFrame.Navigate(page);
        }
    }
}