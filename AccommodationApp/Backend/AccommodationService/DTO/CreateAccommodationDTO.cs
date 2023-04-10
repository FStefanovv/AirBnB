using Accommodation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accommodation.DTO
{
    public class CreateAccommodationDTO
    {
        public string Name { get; set; }
        public Address Address { get; set; }
        public string[] Offers { get; set; }
        public int MinGuests { get; set; }
        public int MaxGuests { get; set; }

        public CreateAccommodationDTO() { }
    }
}
