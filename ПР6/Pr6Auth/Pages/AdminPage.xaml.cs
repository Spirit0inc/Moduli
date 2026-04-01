using System.Windows;
using System.Windows.Controls;
using Pr6Auth.Model;
using Pr6Auth.Services;

namespace Pr6Auth.Pages
{
    public partial class AdminPage : Page
    {
        public AdminPage(Users user)
        {
            InitializeComponent();
            string greeting = TimeHelper.GetGreeting(user.LastName, user.FirstName, user.MiddleName);
            tbWelcome.Text = greeting;
        }

        private void BtnManageEmployees_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EmployeesList());
        }

        private void BtnLogoutClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Autho());
        }
    }
}