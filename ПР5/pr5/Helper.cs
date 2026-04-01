using System;
using System.Linq;
using pr5.Model;

namespace pr5
{
    internal class Helper
    {
        private static pr5DBEntities _context;

        public static pr5DBEntities GetContext()
        {
            if (_context == null)
            {
                _context = new pr5DBEntities();
            }
            return _context;
        }

        public void AddUser(Users user)
        {
            var context = GetContext();  // ← получаем контекст
            context.Users.Add(user);
            context.SaveChanges();
        }

        public IQueryable<Users> GetAllUsers()
        {
            var context = GetContext();  // ← получаем контекст
            return context.Users;
        }
    }
}