using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.DTOs
{
    public class SuccessfulRegistrationDTO
    {

        public string Username { get; set; }

        public SuccessfulRegistrationDTO()
        {

        }

        public SuccessfulRegistrationDTO(string username)
        {
            Username = username;
        }
    }
}
