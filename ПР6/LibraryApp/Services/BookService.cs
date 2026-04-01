using System.Collections.Generic;
using System.Linq;
using LibraryApp.Models;
using LibraryApp.Validators;

namespace LibraryApp.Services
{
    public class BookService : IBookService
    {
        private readonly List<Book> _books = new List<Book>();
        private readonly BookValidator _validator = new BookValidator();

        public bool AddBook(Book book)
        {
            var validationResults = _validator.Validate(book);
            if (validationResults.Any())
            {
                foreach (var result in validationResults)
                    System.Console.WriteLine(result.ErrorMessage);
                return false;
            }

            if (_books.Any(b => b.Title == book.Title))
            {
                System.Console.WriteLine("Книга с таким названием уже существует.");
                return false;
            }

            _books.Add(book);
            return true;
        }

        public Book GetBook(string title)
        {
            return _books.FirstOrDefault(b => b.Title == title);
        }
    }
}