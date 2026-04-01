using LibraryApp.Models;

namespace LibraryApp.Services
{
    public interface IUserService
    {
        bool RegisterUser(User user);
        User GetUser(string username);
    }
}