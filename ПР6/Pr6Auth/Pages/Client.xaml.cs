using System.Windows;
using System.Windows.Controls;

namespace Pr6Auth.Pages
{
    public partial class Client : Page
    {
        public Client(string userName, string role)
        {
            InitializeComponent();

            if (userName == "Гость")
            {
                tbWelcome.Text = "Вы вошли как гость\nДоступ ограничен";
            }
            else
            {
                tbWelcome.Text = $"Добро пожаловать, {userName}!\nВаша роль: {role}";
            }
        }

        private void BtnLogoutClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Autho());
        }
    }
}