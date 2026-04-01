using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class Book
    {
        [Required(ErrorMessage = "Название книги обязательно")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Название от 1 до 100 символов")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Автор обязателен")]
        [StringLength(50, ErrorMessage = "Автор не более 50 символов")]
        public string Author { get; set; }

        [Range(1, 2000, ErrorMessage = "Количество страниц от 1 до 2000")]
        public int Pages { get; set; }
    }
}