using System.Windows;
using System.Windows.Controls;
using Pr6Auth.Model;
using Pr6Auth.Services;

namespace Pr6Auth.Pages
{
    public partial class Client : Page
    {
        public Client(Users user)
        {
            InitializeComponent();
            if (user != null)
            {
                string greeting = TimeHelper.GetGreeting(user.LastName, user.FirstName, user.MiddleName);
                tbWelcome.Text = greeting;
            }
            else
            {
                tbWelcome.Text = "Добро пожаловать, гость!";
            }
        }

        private void BtnLogoutClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Autho());
        }
    }
}