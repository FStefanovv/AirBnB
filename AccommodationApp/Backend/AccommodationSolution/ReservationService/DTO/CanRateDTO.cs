using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationService.DTO
{
    public class CanRateDTO
    {
        public bool Host { get; set; }
        public bool Accommodation { get; set; }

        public CanRateDTO() { }

    }
}
