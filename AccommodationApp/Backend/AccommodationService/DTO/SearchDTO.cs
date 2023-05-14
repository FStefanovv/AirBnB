using Accommodation.Model;
using System;

namespace Accommodation.DTO
{
    public class SearchDTO
    {
        public Address Location { get; set; }
        public int numberOfGuests { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public SearchDTO() { }
    }
}
