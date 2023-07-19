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
        public bool AutoApprove { get; set; }
        public string StartSeason { get; set; }
        public string EndSeason { get; set; }
        public double Price { get; set; }
        public bool PricePerGuest { get; set; }
        public bool PricePerAccomodation { get; set; }
        public bool HolidayCost { get; set; }
        public bool WeekendCost { get; set; }
        public bool SummerCost { get; set; }



        public CreateAccommodationDTO() { }
    }
}
