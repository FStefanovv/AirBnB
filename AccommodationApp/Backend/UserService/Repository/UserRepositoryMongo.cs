
﻿using Amazon.SecurityToken.Model;
using Microsoft.AspNetCore.Identity;
﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.DTO;
using Users.Model;
using Users.RabbitMQ;

namespace Users.Repository
{
    public class UserRepositoryMongo : IUserRepository
    {
        private readonly IDbContext _context;
        private IMongoCollection<User> _users;
        private PasswordHasher<User> _hasher;

        public UserRepositoryMongo(IDbContext context)
        {
            _context = context;
            _users = _context.GetCollection<User>("users");
            _hasher = new PasswordHasher<User>();
        }

        public void Create(User user)
        {
            _users.InsertOne(user);
        }


        public User GetUserWithCredentials(LoginCredentialsDTO credentials)
        {
            foreach (User user in _users.Find(user => true).ToList())
            {
                if (user.Username == credentials.Username && _hasher.VerifyHashedPassword(user, user.Password, credentials.Password) != 0)
                    return user;
            }
            return null;
        }


        public bool CheckIfUsernameInUse(string username)
        {
            return _users.Find(user => user.Username == username).FirstOrDefault() != null;
        }

        public User GetUserById(string id)
        {
            return _users.Find(user => user.Id == id).FirstOrDefault();
        }

        public bool CheckIfEMailInUse(string email)
        {
            return _users.Find(user => user.Email == email).FirstOrDefault() != null;
        }

        public User GetUser(StringValues userId)
        {
            return _users.Find(user => user.Id == userId).FirstOrDefault();
        }

        public void UpdateUser(User user)
        {
            _users.InsertOne(user);
        }

        public void Delete(User user)
        {

            var filter = Builders<User>.Filter.Eq("Id", user.Id);

            _users.DeleteOne(filter);
        }

        public User GetById(StringValues id)
        {
           return (User)_users.Find(user=>user.Id==id);
        }

        public bool UpdateUserSaga(string id, SagaState state)
        {
            throw new NotImplementedException();
        }
    }

}
