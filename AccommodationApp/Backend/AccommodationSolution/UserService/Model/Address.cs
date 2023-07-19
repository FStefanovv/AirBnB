using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Users.Model
{
    [Owned]
    public class Address
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public Address() { }
        public Address(string country, string city, string street, int number){
            Country = country;
            City = city;
            Street = street;
            Number = number;
        }
    }
}
