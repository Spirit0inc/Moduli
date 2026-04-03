using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Pr6Auth.Model;
using Pr6Auth.Services;

namespace Pr6Auth.Pages
{
    public partial class Register : Page
    {
        public Register()
        {
            InitializeComponent();
        }

        private void BtnRegisterClick(object sender, RoutedEventArgs e)
        {
            string lastName = tbLastName.Text.Trim();
            string firstName = tbFirstName.Text.Trim();
            string middleName = tbMiddleName.Text.Trim();
            string login = tbLogin.Text.Trim();
            string password = pbPassword.Password;
            string confirm = pbConfirmPassword.Password;
            string email = tbEmail.Text.Trim();

            // Проверка обязательных полей
            if (string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(firstName))
            {
                ShowError("Фамилия и имя обязательны!");
                return;
            }
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                ShowError("Заполните все поля!");
                return;
            }
            if (password != confirm)
            {
                ShowError("Пароли не совпадают!");
                return;
            }
            if (password.Length < 4)
            {
                ShowError("Пароль должен быть не менее 4 символов!");
                return;
            }
            if (string.IsNullOrEmpty(email))
            {
                ShowError("Email обязателен для восстановления пароля!");
                return;
            }

            ComboBoxItem selectedItem = cbRole.SelectedItem as ComboBoxItem;
            string role = (selectedItem.Tag as string) == "admin" ? "admin" : "user";

            try
            {
                using (var db = new pr5DBEntities1())
                {
                    // Проверка уникальности логина
                    var existingUser = db.Users.FirstOrDefault(u => u.Login == login);
                    if (existingUser != null)
                    {
                        ShowError("Пользователь с таким логином уже существует!");
                        return;
                    }

                    // Проверка уникальности email
                    var existingEmail = db.Users.FirstOrDefault(u => u.Email == email);
                    if (existingEmail != null)
                    {
                        ShowError("Пользователь с таким email уже существует!");
                        return;
                    }

                    string hashedPassword = Hash.HashPassword(password);

                    var newUser = new Users
                    {
                        Login = login,
                        PasswordHash = hashedPassword,
                        Role = role,
                        LastName = lastName,
                        FirstName = firstName,
                        MiddleName = string.IsNullOrEmpty(middleName) ? null : middleName,
                        Email = email
                    };

                    db.Users.Add(newUser);
                    db.SaveChanges();

                    MessageBox.Show($"Пользователь {login} успешно зарегистрирован!",
                                    "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    NavigationService.Navigate(new Autho());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Autho());
        }

        private void ShowError(string message)
        {
            tbError.Text = message;
            tbError.Visibility = Visibility.Visible;
        }
    }
}