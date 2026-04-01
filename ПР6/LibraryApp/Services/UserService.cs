using System.Collections.Generic;
using System.Linq;
using LibraryApp.Models;
using LibraryApp.Validators;

namespace LibraryApp.Services
{
    public class UserService : IUserService
    {
        private readonly List<User> _users = new List<User>();
        private readonly UserValidator _validator = new UserValidator();

        public bool RegisterUser(User user)
        {
            var validationResults = _validator.Validate(user);
            if (validationResults.Any())
            {
                foreach (var result in validationResults)
                    System.Console.WriteLine(result.ErrorMessage);
                return false;
            }

            if (_users.Any(u => u.Username == user.Username))
            {
                System.Console.WriteLine("Пользователь с таким именем уже существует.");
                return false;
            }

            _users.Add(user);
            return true;
        }

        public User GetUser(string username)
        {
            return _users.FirstOrDefault(u => u.Username == username);
        }
    }
}