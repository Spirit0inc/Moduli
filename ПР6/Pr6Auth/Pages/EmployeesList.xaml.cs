using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Pr6Auth.Model;

namespace Pr6Auth.Pages
{
    public partial class EmployeesList : Page
    {
        private pr5DBEntities1 db; // убрали readonly, чтобы можно было пересоздавать контекст

        public EmployeesList()
        {
            InitializeComponent();
            db = new pr5DBEntities1();
            LoadPositionsFilter();
            LoadEmployees();

            // Подписываемся на событие навигации (при возврате на страницу)
            if (NavigationService != null)
                NavigationService.Navigated += NavigationService_Navigated;
        }

        // Срабатывает при каждом переходе на страницу (включая возврат)
        private void NavigationService_Navigated(object sender, NavigationEventArgs e)
        {
            // Если текущая страница (эта) стала активной
            if (e.Content == this)
            {
                // Обновляем контекст (пересоздаём, чтобы данные были свежими)
                db?.Dispose();
                db = new pr5DBEntities1();
                LoadPositionsFilter();
                LoadEmployees();
            }
        }

        // Очистка при выгрузке страницы
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null)
                NavigationService.Navigated -= NavigationService_Navigated;
            db?.Dispose();
        }

        private void LoadPositionsFilter()
        {
            var positions = db.Employees.Select(e => e.Position).Distinct().ToList();
            positions.Insert(0, "Все");
            cbPositionFilter.ItemsSource = positions;
            cbPositionFilter.SelectedIndex = 0;
        }

        private void LoadEmployees()
        {
            var query = db.Employees.AsQueryable();

            string search = tbSearch.Text.Trim();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e => (e.LastName + " " + e.FirstName + " " + e.MiddleName).Contains(search));
            }

            string selectedPosition = cbPositionFilter.SelectedItem as string;
            if (!string.IsNullOrEmpty(selectedPosition) && selectedPosition != "Все")
            {
                query = query.Where(e => e.Position == selectedPosition);
            }

            dgEmployees.ItemsSource = query.OrderBy(e => e.LastName).ToList();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e) => LoadEmployees();

        private void cbPositionFilter_SelectionChanged(object sender, SelectionChangedEventArgs e) => LoadEmployees();

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            tbSearch.Text = "";
            cbPositionFilter.SelectedIndex = 0;
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EmployeeEdit(null));
        }

        private void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                var employee = db.Employees.Find(id);
                NavigationService.Navigate(new EmployeeEdit(employee));
            }
        }

        private void dgEmployees_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dgEmployees.SelectedItem is Employees employee)
            {
                NavigationService.Navigate(new EmployeeEdit(employee));
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}