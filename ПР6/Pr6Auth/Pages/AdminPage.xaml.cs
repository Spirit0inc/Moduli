using System.Windows;
using System.Windows.Controls;

namespace Pr6Auth.Pages
{
    public partial class AdminPage : Page
    {
        public AdminPage(string userName, string role)
        {
            InitializeComponent();
            tbWelcome.Text = $"Вы вошли как: {userName}\nВаша роль: {role}";
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Autho());
        }
    }
}