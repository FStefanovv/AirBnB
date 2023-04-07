using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Users.DTO
{
    public class TokenDTO
    {
        public string Token { get; set; }

        public TokenDTO() { }
        public TokenDTO(string token)
        {
            Token = token;
        }
    }
}
