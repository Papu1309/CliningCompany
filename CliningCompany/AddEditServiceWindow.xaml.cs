using System;
using System.Linq;
using System.Windows;
using CliningCompany.Connect;

namespace CliningCompany
{
    public partial class AddEditServiceWindow : Window
    {
        private Services _service;
        private bool isEdit = false;

        public AddEditServiceWindow()
        {
            InitializeComponent();
            LoadCategories();
            _service = new Services();
            isEdit = false;
        }

        public AddEditServiceWindow(Services service)
        {
            InitializeComponent();
            LoadCategories();
            _service = service;
            cmbCategory.SelectedValue = service.CategoryId;
            txtName.Text = service.Name;
            txtPrice.Text = service.Price.ToString();
            txtDescription.Text = service.Description;
            isEdit = true;
        }

        private void LoadCategories()
        {
            cmbCategory.ItemsSource = Connection.entities.Categories.ToList();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbCategory.SelectedItem == null)
            {
                MessageBox.Show("Выберите категорию!");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Заполните название и цену!");
                return;
            }
            if (!decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Цена должна быть числом!");
                return;
            }

            _service.CategoryId = (int)cmbCategory.SelectedValue;
            _service.Name = txtName.Text.Trim();
            _service.Price = price;
            _service.Description = txtDescription.Text.Trim();

            if (!isEdit)
            {
                Connection.entities.Services.Add(_service);
            }
            Connection.entities.SaveChanges();
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}