﻿using Flights.DTOs;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Model
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRequired]
        public string FirstName { get; set; }
        [BsonRequired]
        public string LastName { get; set; }
        [BsonIgnore]
        public string FullName { get { return FirstName + " " + LastName;  } }
        [BsonRequired]
        public string Username { get; set; }
        [BsonRequired]
        public string EMail { get; set; }
        [BsonRequired]
        public string Password { get; set; }
        [BsonRequired]
        public string Role { get; set; }


        public User() { }

        public User(RegistrationDTO userData)
        {
            FirstName = userData.FirstName;
            LastName = userData.LastName;
            Username = userData.Username;
            EMail = userData.EMail;
            Password = userData.Password;
            Role = "REGULAR_USER";
        }       
    }
}
