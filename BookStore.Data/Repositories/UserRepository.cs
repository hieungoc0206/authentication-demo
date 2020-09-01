using BookStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BookStore.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private BookStoreDbContext _context;

        public UserRepository(BookStoreDbContext context)
        {
            _context = context;
            if (!_context.Users.Any())
            {
                _context.Users.Add(new User()
                {
                    Id = 1,
                    UserName = "hieungoc",
                    FirstName = "Hieu",
                    LastName = "Nguyen",
                    FavoriteColor = "Black",
                    Password = "6a663550999d5f26a68392b9b20e5f65eb360e24a3d56ed4614cca75a8e4a3f5",
                    Role = "Admin"
                });
                _context.SaveChanges();
            }
        }

        public User GetByUserNameAndPassword(string userName, string password)
        {
            var hash = ComputeSha256Hash(password);
            return _context.Users.FirstOrDefault(u => u.UserName == userName && u.Password == hash);
        }

        private string ComputeSha256Hash(string pass)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(pass));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }

    public interface IUserRepository
    {
        User GetByUserNameAndPassword(string userName, string password);
    }
}
