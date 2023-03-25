using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.DTOs
{
    public class LoginCredentialsDTO
    {
        public string Username { get; }
        public string Password { get; }

        public LoginCredentialsDTO() { }
    }
}
