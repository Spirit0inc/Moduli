using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Pr6Auth.Model;
using Pr6Auth.Services;

namespace Pr6Auth.Pages
{
    public partial class Autho : Page
    {
        private int attemptCount = 0;
        private string currentCaptcha = "";

        public Autho()
        {
            InitializeComponent();
            HideCaptcha();
        }

        private void HideCaptcha()
        {
            tblCaptcha.Visibility = Visibility.Collapsed;
            tbCaptcha.Visibility = Visibility.Collapsed;
        }

        private void ShowCaptcha()
        {
            currentCaptcha = CaptchaGenerator.GenerateCaptchaText(6);
            tblCaptcha.Text = $"Введите: {currentCaptcha}";
            tblCaptcha.Visibility = Visibility.Visible;
            tbCaptcha.Visibility = Visibility.Visible;
            tbCaptcha.Text = "";
        }

        private void ClearAll()
        {
            tbLogin.Text = "";
            pbPassword.Password = "";
            tbCaptcha.Text = "";
        }

        private bool IsEmployeeRole(string role)
        {
            string r = role.ToLower();
            return r == "admin" || r == "manager" || r == "администратор" || r == "менеджер";
        }

        private void BtnGuestClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Client(null));
        }

        private void BtnRegisterClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Register());
        }

        private void BtnLoginClick(object sender, RoutedEventArgs e)
        {
            string login = tbLogin.Text.Trim();
            string password = pbPassword.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            attemptCount++;
            string hashedPassword = Hash.HashPassword(password);

            try
            {
                using (var db = new pr5DBEntities1())
                {
                    var user = db.Users.FirstOrDefault(u => u.Login == login && u.PasswordHash == hashedPassword);

                    if (user != null)
                    {
                        // Проверка рабочего времени для сотрудников
                        if (IsEmployeeRole(user.Role) && !TimeHelper.IsWorkingTime(DateTime.Now))
                        {
                            MessageBox.Show("Доступ в систему разрешён только в рабочее время (10:00-19:00).",
                                            "Доступ ограничен", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return; // прерываем вход
                        }

                        MessageBox.Show($"Добро пожаловать, {user.Login}!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Переход на страницу в зависимости от роли
                        if (IsEmployeeRole(user.Role))
                        {
                            NavigationService.Navigate(new AdminPage(user));
                        }
                        else
                        {
                            NavigationService.Navigate(new Client(user));
                        }
                    }
                    else
                    {
                        if (attemptCount >= 2)
                        {
                            ShowCaptcha();
                            if (attemptCount > 2 && tbCaptcha.Text != currentCaptcha)
                            {
                                MessageBox.Show("Неверная капча!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                ClearAll();
                                ShowCaptcha();
                                return;
                            }
                        }
                        MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        ClearAll();
                        pbPassword.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к БД: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}