using System;
using System.Windows;
using System.Windows.Controls;
using Pr6Auth.Model;
using Pr6Auth.Services;

namespace Pr6Auth.Pages
{
    public partial class ResetPasswordPage : Page
    {
        private readonly Users user;

        public ResetPasswordPage(Users user)
        {
            InitializeComponent();
            this.user = user;
        }

        private void SavePassword_Click(object sender, RoutedEventArgs e)
        {
            string newPass = pbNewPassword.Password;
            string confirm = pbConfirmPassword.Password;

            if (string.IsNullOrEmpty(newPass) || newPass.Length < 4)
            {
                MessageBox.Show("Пароль должен быть не менее 4 символов!",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (newPass != confirm)
            {
                MessageBox.Show("Пароли не совпадают!",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new pr5DBEntities1())
                {
                    var dbUser = db.Users.Find(user.Id);
                    dbUser.PasswordHash = Hash.HashPassword(newPass);
                    db.SaveChanges();
                }

                MessageBox.Show("Пароль успешно изменён!",
                                "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new Autho());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Autho());
        }
    }
}