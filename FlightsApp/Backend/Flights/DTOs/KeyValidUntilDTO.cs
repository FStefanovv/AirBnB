using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.DTOs
{
    public class KeyValidUntilDTO
    {
        public KeyValidUntilDTO() { }

        public DateTime ValidUntil { get; set; }
        public string UserId { get; set; }
    }
}
