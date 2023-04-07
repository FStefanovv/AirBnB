﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Users.Model
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //field should not be persisted in database
        public string FullName { get { return FirstName + " " + LastName; } }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }


        public User() { }

        public User(User user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Password = user.Password;
            Role = user.Role;
        }
    }
}
