using System;
using System.Linq;
using System.Threading.Tasks;
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
        private string resetCode = "";
        private string resetUserEmail = "";
        private Users resettingUser = null;
        private string twoFACode = "";
        private Users twoFAUser = null;

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
            tblCaptcha.Text = $"Введите капчу: {currentCaptcha}";
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

        // ========== ВОССТАНОВЛЕНИЕ ПАРОЛЯ ==========
        private async void BtnForgotPasswordClick(object sender, RoutedEventArgs e)
        {
            string loginOrEmail = tbLogin.Text.Trim();
            if (string.IsNullOrEmpty(loginOrEmail))
            {
                MessageBox.Show("Введите логин или email для восстановления пароля.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            using (var db = new pr5DBEntities1())
            {
                var user = db.Users.FirstOrDefault(u => u.Login == loginOrEmail || u.Email == loginOrEmail);
                if (user == null || string.IsNullOrEmpty(user.Email))
                {
                    MessageBox.Show("Пользователь не найден или у него не указан email.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                resetCode = new Random().Next(1000, 9999).ToString();
                resetUserEmail = user.Email;
                resettingUser = user;

                var emailService = new EmailService();
                bool sent = await emailService.SendCodeAsync(resetUserEmail, resetCode, "Восстановление пароля");

                if (sent)
                {
                    MessageBox.Show($"Код отправлен на {resetUserEmail}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    ShowResetCodeInput();
                }
            }
        }

        private void ShowResetCodeInput()
        {
            tbLogin.Visibility = Visibility.Collapsed;
            pbPassword.Visibility = Visibility.Collapsed;
            btnLogin.Visibility = Visibility.Collapsed;
            btnGuest.Visibility = Visibility.Collapsed;
            btnRegister.Visibility = Visibility.Collapsed;
            btnForgotPassword.Visibility = Visibility.Collapsed;
            tbl2FACode.Visibility = Visibility.Collapsed;
            tb2FACode.Visibility = Visibility.Collapsed;
            btnConfirm2FA.Visibility = Visibility.Collapsed;

            tblCaptcha.Text = "Введите код из письма:";
            tblCaptcha.Visibility = Visibility.Visible;
            tbCaptcha.Visibility = Visibility.Visible;
            tbCaptcha.Width = 150;
            btnLogin.Visibility = Visibility.Visible;
            btnLogin.Content = "Подтвердить код";
            btnLogin.Click -= BtnLoginClick;
            btnLogin.Click += BtnConfirmResetCodeClick;
        }

        private async void BtnConfirmResetCodeClick(object sender, RoutedEventArgs e)
        {
            if (tbCaptcha.Text == resetCode)
            {
                NavigationService.Navigate(new ResetPasswordPage(resettingUser));
            }
            else
            {
                MessageBox.Show("Неверный код подтверждения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ========== ДВУХФАКТОРНАЯ АУТЕНТИФИКАЦИЯ ==========
        private void Show2FAInput()
        {
            tbLogin.Visibility = Visibility.Collapsed;
            pbPassword.Visibility = Visibility.Collapsed;
            btnLogin.Visibility = Visibility.Collapsed;
            btnGuest.Visibility = Visibility.Collapsed;
            btnRegister.Visibility = Visibility.Collapsed;
            btnForgotPassword.Visibility = Visibility.Collapsed;
            tblCaptcha.Visibility = Visibility.Collapsed;
            tbCaptcha.Visibility = Visibility.Collapsed;

            tbl2FACode.Visibility = Visibility.Visible;
            tb2FACode.Visibility = Visibility.Visible;
            btnConfirm2FA.Visibility = Visibility.Visible;
        }

        private void BtnConfirm2FAClick(object sender, RoutedEventArgs e)
        {
            if (tb2FACode.Text == twoFACode)
            {
                MessageBox.Show($"Добро пожаловать, {twoFAUser.Login}!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                if (IsEmployeeRole(twoFAUser.Role))
                    NavigationService.Navigate(new AdminPage(twoFAUser));
                else
                    NavigationService.Navigate(new Client(twoFAUser));
            }
            else
            {
                MessageBox.Show("Неверный код подтверждения!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                tb2FACode.Text = "";
            }
        }

        // ========== ОСНОВНОЙ ВХОД (АСИНХРОННЫЙ) ==========
        private async void BtnLoginClick(object sender, RoutedEventArgs e)
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

            using (var db = new pr5DBEntities1())
            {
                var user = db.Users.FirstOrDefault(u => u.Login == login && u.PasswordHash == hashedPassword);

                if (user != null)
                {
                    if (!string.IsNullOrEmpty(user.Email))
                    {
                        twoFACode = new Random().Next(1000, 9999).ToString();
                        twoFAUser = user;

                        var emailService = new EmailService();
                        bool sent = await emailService.SendCodeAsync(user.Email, twoFACode, "Двухфакторная аутентификация");

                        if (sent)
                        {
                            MessageBox.Show($"Код подтверждения отправлен на {user.Email}", "2FA", MessageBoxButton.OK, MessageBoxImage.Information);
                            Show2FAInput();
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Добро пожаловать, {user.Login}!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        if (IsEmployeeRole(user.Role))
                            NavigationService.Navigate(new AdminPage(user));
                        else
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
    }
}