using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HashLibrary;
using pr5.Model;  // ← правильное пространство имен

namespace pr5
{
    public partial class MainWindow : Window
    {
        private Helper helper = new Helper();
        private PasswordHasher hasher = new PasswordHasher();

        public MainWindow()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем логин
            if (string.IsNullOrWhiteSpace(txtLogin.Text))
            {
                MessageBox.Show("Введите логин!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Проверяем пароль
            if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Введите пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Хешируем пароль
            string hashedPassword = hasher.GetHash(txtPassword.Password);

            // Получаем выбранную роль
            string role = "User";
            if (cmbRole.SelectedItem is ComboBoxItem selectedItem)
            {
                role = selectedItem.Content.ToString();
            }

            // Создаем пользователя
            var newUser = new Users  // ← Users из пространства pr5.Model
            {
                Login = txtLogin.Text,
                PasswordHash = hashedPassword,
                Role = role
            };

            try
            {
                helper.AddUser(newUser);
                MessageBox.Show("Пользователь зарегистрирован!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                // Очищаем поля
                txtLogin.Clear();
                txtPassword.Clear();
                cmbRole.SelectedIndex = 0;

                // Обновляем список
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnShowAll_Click(object sender, RoutedEventArgs e)
        {
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                var users = helper.GetAllUsers().ToList();
                dgvUsers.ItemsSource = users;

                // Скрываем столбец с хешем пароля
                foreach (var column in dgvUsers.Columns)
                {
                    if (column.Header.ToString() == "PasswordHash")
                    {
                        column.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}