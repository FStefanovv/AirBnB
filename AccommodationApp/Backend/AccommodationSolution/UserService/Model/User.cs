using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Users.RabbitMQ;

namespace Users.Model
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [NotMapped]
        public string FullName { get { return FirstName + " " + LastName; } }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public Address Address { get; set; }
        public bool IsDistinguishedHost { get; set; }
        public bool IsReservationPartSatisfied { get; set; }
        public bool IsRatingPartSatisfied { get; set; }

        public SagaState State { get; set; }


        public User() { }

        public User(string firstName, string lastName, string email, string password, string role)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Role = role;
            IsDistinguishedHost = false;
            IsReservationPartSatisfied = false;
            IsRatingPartSatisfied = false;
        }
    }
}
