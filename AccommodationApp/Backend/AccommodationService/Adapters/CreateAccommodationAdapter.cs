using Accommodation.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Accommodation.Adapters
{
    public static class CreateAccommodationAdapter
    {
        public static Model.Accommodation CreateAccommodaitonDtoToObject(CreateAccommodationDTO dto, string hostId)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("sr-Cyrl-Rs");
            var accommodation = new Model.Accommodation()
            {
                Name = dto.Name,
                Address = dto.Address,
                Offers = dto.Offers,
                MaxGuests = dto.MaxGuests,
                MinGuests = dto.MinGuests,
                HostId = hostId,
                AutoApprove = dto.AutoApprove,
                StartSeasonDate = DateTime.ParseExact(dto.StartSeason, "dd-MM-yyyy", culture),
                EndSeasonDate = DateTime.ParseExact(dto.EndSeason, "dd-MM-yyyy", culture),
                AccomodationPrice = new Model.Price()
                {
                    FinalPrice = dto.Price,
                    PricePerAccomodation = dto.PricePerAccomodation,
                    PricePerGuest = dto.PricePerGuest,
                    HolidayCost=dto.HolidayCost,
                    WeekendCost=dto.WeekendCost,
                    SummerCost=dto.SummerCost,
                }
            };

            return accommodation;
        }

        public static AccommodationDTO ObjectToAccommodationDTO(Model.Accommodation accommodation)
        {
            var accommodationDTO = new AccommodationDTO()
            {
                Name = accommodation.Name,
                StartSeason = accommodation.StartSeasonDate.ToString("dd-MM-yyyy"),
                EndSeason = accommodation.EndSeasonDate.ToString("dd-MM-yyyy"),
                Price = accommodation.AccomodationPrice.FinalPrice,
                PricePerGuest = accommodation.AccomodationPrice.PricePerGuest,
                PricePerAccomodation = accommodation.AccomodationPrice.PricePerAccomodation,
                HolidayCost = accommodation.AccomodationPrice.HolidayCost,
                WeekendCost = accommodation.AccomodationPrice.WeekendCost,
                SummerCost = accommodation.AccomodationPrice.SummerCost

            };

            return accommodationDTO;
        }
    }
}
