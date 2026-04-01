using System;
using LibraryApp.Models;
using LibraryApp.Services;
using LibraryApp.Utilities;

namespace LibraryApp
{
    class Program
    {
        private static UserService userService = new UserService();
        private static BookService bookService = new BookService();
        private static ReservationService reservationService = new ReservationService();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Библиотечная система ===");
                Console.WriteLine("1. Регистрация пользователя");
                Console.WriteLine("2. Добавление книги");
                Console.WriteLine("3. Резервирование книги");
                Console.WriteLine("4. Выход");
                Console.Write("Выберите действие: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        RegisterUser();
                        break;
                    case "2":
                        AddBook();
                        break;
                    case "3":
                        ReserveBook();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Нажмите любую клавишу...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void RegisterUser()
        {
            Console.Clear();
            Console.WriteLine("=== Регистрация пользователя ===");
            string username = ConsoleInput.ReadLine("Имя пользователя: ");
            string password = ConsoleInput.ReadLine("Пароль: ");
            string fullName = ConsoleInput.ReadLine("Полное имя: ");
            string email = ConsoleInput.ReadLine("Email: ");

            var user = new User
            {
                Username = username,
                Password = password,
                FullName = fullName,
                Email = email
            };

            if (userService.RegisterUser(user))
            {
                Console.WriteLine("Пользователь успешно зарегистрирован!");
            }
            else
            {
                Console.WriteLine("Не удалось зарегистрировать пользователя.");
            }
            Console.WriteLine("Нажмите любую клавишу...");
            Console.ReadKey();
        }

        static void AddBook()
        {
            Console.Clear();
            Console.WriteLine("=== Добавление книги ===");
            string title = ConsoleInput.ReadLine("Название книги: ");
            string author = ConsoleInput.ReadLine("Автор: ");
            int pages = 0;
            Console.Write("Количество страниц: ");
            int.TryParse(Console.ReadLine(), out pages);

            var book = new Book
            {
                Title = title,
                Author = author,
                Pages = pages
            };

            if (bookService.AddBook(book))
            {
                Console.WriteLine("Книга успешно добавлена!");
            }
            else
            {
                Console.WriteLine("Не удалось добавить книгу.");
            }
            Console.WriteLine("Нажмите любую клавишу...");
            Console.ReadKey();
        }

        static void ReserveBook()
        {
            Console.Clear();
            Console.WriteLine("=== Резервирование книги ===");
            string username = ConsoleInput.ReadLine("Ваше имя пользователя: ");
            string bookTitle = ConsoleInput.ReadLine("Название книги: ");

            var user = userService.GetUser(username);
            if (user == null)
            {
                Console.WriteLine("Пользователь не найден.");
                Console.WriteLine("Нажмите любую клавишу...");
                Console.ReadKey();
                return;
            }

            var book = bookService.GetBook(bookTitle);
            if (book == null)
            {
                Console.WriteLine("Книга не найдена.");
                Console.WriteLine("Нажмите любую клавишу...");
                Console.ReadKey();
                return;
            }

            var reservation = new Reservation
            {
                User = user,
                Book = book,
                ReservationDate = DateTime.Now
            };

            if (reservationService.ReserveBook(reservation))
            {
                Console.WriteLine("Книга успешно зарезервирована!");
            }
            else
            {
                Console.WriteLine("Не удалось зарезервировать книгу.");
            }
            Console.WriteLine("Нажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}