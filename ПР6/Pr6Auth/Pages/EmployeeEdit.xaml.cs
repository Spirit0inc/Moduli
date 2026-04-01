using Pr6Auth.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Pr6Auth.Pages
{
    public partial class EmployeeEdit : Page
    {
        private readonly Employees editingEmployee;

        public EmployeeEdit(Employees employee)
        {
            InitializeComponent();
            editingEmployee = employee;

            if (employee != null)
            {
                tbLastName.Text = employee.LastName;
                tbFirstName.Text = employee.FirstName;
                tbMiddleName.Text = employee.MiddleName;
                tbPosition.Text = employee.Position;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string lastName = tbLastName.Text.Trim();
            string firstName = tbFirstName.Text.Trim();
            string middleName = tbMiddleName.Text.Trim();
            string position = tbPosition.Text.Trim();

            // Простые проверки обязательных полей
            if (string.IsNullOrWhiteSpace(lastName))
            {
                MessageBox.Show("Фамилия обязательна!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(firstName))
            {
                MessageBox.Show("Имя обязательно!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(position))
            {
                MessageBox.Show("Должность обязательна!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Employees employeeToSave = editingEmployee ?? new Employees();

            employeeToSave.LastName = lastName;
            employeeToSave.FirstName = firstName;
            employeeToSave.MiddleName = string.IsNullOrWhiteSpace(middleName) ? null : middleName;
            employeeToSave.Position = position;

            // Дополнительная валидация через DataAnnotations
            var validationContext = new ValidationContext(employeeToSave);
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            if (!Validator.TryValidateObject(employeeToSave, validationContext, validationResults, true))
            {
                string errors = string.Join(Environment.NewLine, validationResults.Select(r => r.ErrorMessage));
                MessageBox.Show(errors, "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Сохраняем в БД с новым контекстом
            using (var db = new pr5DBEntities1())   // замени на имя твоего контекста
            {
                try
                {
                    if (editingEmployee == null)
                    {
                        db.Employees.Add(employeeToSave);
                    }
                    else
                    {
                        db.Entry(employeeToSave).State = EntityState.Modified;
                    }

                    db.SaveChanges();
                    MessageBox.Show("Сотрудник сохранён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.GoBack();
                }
                catch (DbEntityValidationException ex)
                {
                    var errorMessages = ex.EntityValidationErrors
                        .SelectMany(ve => ve.ValidationErrors)
                        .Select(ve => $"{ve.PropertyName}: {ve.ErrorMessage}");
                    string message = string.Join(Environment.NewLine, errorMessages);
                    MessageBox.Show($"Ошибка валидации EF:\n{message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();
    }
}