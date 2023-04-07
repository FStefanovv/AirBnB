using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Users.DTO
{
    public class SuccessfulRegistrationDTO
    {
        public string Email { get; set; }

        public SuccessfulRegistrationDTO()
        {

        }

        public SuccessfulRegistrationDTO(string email)
        {
            Email = email;
        }
    }
}
