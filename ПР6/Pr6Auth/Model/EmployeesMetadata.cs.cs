using System.ComponentModel.DataAnnotations;

namespace Pr6Auth.Model
{
    // Метаданные для класса Employees (добавляем правила валидации)
    public class EmployeesMetadata
    {
        [Required(ErrorMessage = "Фамилия обязательна")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Фамилия должна быть от 2 до 50 символов")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Имя обязательно")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Имя должно быть от 2 до 50 символов")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Отчество не может быть длиннее 50 символов")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Должность обязательна")]
        [StringLength(100, ErrorMessage = "Должность не может быть длиннее 100 символов")]
        public string Position { get; set; }
    }

    // Привязываем метаданные к классу Employees (он уже существует в модели)
    [MetadataType(typeof(EmployeesMetadata))]
    public partial class Employees
    {
        // Этот класс пустой, он объединится с автоматическим классом из .edmx
    }
}