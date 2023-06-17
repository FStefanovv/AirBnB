using Accommodation.Model;
using System;

namespace Accommodation.DTO
{
    public class SearchDTO
    {
        public String Location { get; set; }
        public int NumberOfGuests { get; set; }
        public String CheckIn { get; set; }
        public String CheckOut { get; set; }
        public SearchDTO() { }
    }
}
