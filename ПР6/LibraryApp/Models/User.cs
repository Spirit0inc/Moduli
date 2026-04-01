using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class User
    {
        [Required(ErrorMessage = "Имя пользователя обязательно")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Имя должно быть от 3 до 20 символов")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Пароль должен быть от 4 до 50 символов")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Полное имя обязательно")]
        [StringLength(50, ErrorMessage = "Полное имя не более 50 символов")]
        public string FullName { get; set; }

        [EmailAddress(ErrorMessage = "Некорректный адрес электронной почты")]
        public string Email { get; set; }
    }
}