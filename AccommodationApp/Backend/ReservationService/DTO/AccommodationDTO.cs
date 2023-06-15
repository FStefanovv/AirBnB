using ReservationService.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationService.DTO
{
    public class AccommodationDTO
    {
        public string Name { get; set; }
        public string StartSeason { get; set; }
        public string EndSeason { get; set; }
        public double Price { get; set; }
        public bool PricePerGuest { get; set; }
        public bool PricePerAccomodation { get; set; }
        public bool HolidayCost { get; set; }
        public bool WeekendCost { get; set; }
        public bool SummerCost { get; set; }

        public AccommodationDTO() { }
    }
}
