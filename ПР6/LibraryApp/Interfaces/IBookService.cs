using LibraryApp.Models;

namespace LibraryApp.Services
{
    public interface IBookService
    {
        bool AddBook(Book book);
        Book GetBook(string title);
    }
}