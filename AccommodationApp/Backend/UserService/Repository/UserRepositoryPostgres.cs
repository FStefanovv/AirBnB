using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.DTO;
using Users.Model;

namespace Users.Repository
{
    public class UserRepositoryPostgres : IUserRepository
    {
        private readonly PostgresDbContext _context;
        private PasswordHasher<User> _hasher;


        public UserRepositoryPostgres(PostgresDbContext context)
        {
            _context = context;
            _hasher = new PasswordHasher<User>();

        }

        public void Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public bool CheckIfEMailInUse(string email)
        {
            return _context.Users.Where(user => user.Email == email).FirstOrDefault() != null;
        }

        public bool CheckIfUsernameInUse(string username)
        {
            return _context.Users.Where(user => user.Username == username).FirstOrDefault() != null;

        }

        public User GetUserWithCredentials(LoginCredentialsDTO credentials)
        {
            foreach (User user in _context.Users.ToList())
            {
                if (user.Username == credentials.Username && _hasher.VerifyHashedPassword(user, user.Password, credentials.Password) != 0)
                    return user;
            }
            return null;
        }

        public User GetUser(StringValues userId)
        {
            foreach (User user in _context.Users.ToList())
            {
                if (user.Id == userId)
                    return user;
            }
            return null;
        }

        public User GetUserById(string id)
        {
            foreach (User user in _context.Users.ToList())
            {
                if (user.Id == id)
                    return user;
            }
            return null;
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
        public void Delete (User user)
        {
           
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public User GetById(StringValues id)
        {
            return _context.Users.Find(id);
        }
    }
}
