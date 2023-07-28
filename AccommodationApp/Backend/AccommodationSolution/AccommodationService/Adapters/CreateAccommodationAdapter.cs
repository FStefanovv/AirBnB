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
        public static Model.Accommodation CreateAccommodaitonDtoToObject(DTO.CreateAccommodationDTO dto, string hostId, string hostUsername)
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
                HostUsername = hostUsername, 
                AutoApprove = dto.AutoApprove,
                StartSeasonDate = DateTime.ParseExact(dto.StartSeason, "yyyy-MM-dd", culture),
                EndSeasonDate = DateTime.ParseExact(dto.EndSeason, "yyyy-MM-dd", culture),
                IsDistinguishedHost = dto.IsDistinguishedHost,
                AccomodationPrice = new Model.Price()
                {
                    FinalPrice = dto.Price,
                    PricePerAccomodation = dto.PricePerAccomodation,
                    PricePerGuest = dto.PricePerGuest,
                    HolidayCost = dto.HolidayCost,
                    WeekendCost = dto.WeekendCost,
                    SummerCost = dto.SummerCost,
                }
            };

            return accommodation;
        }

        public static DTO.AccommodationDTO ObjectToAccommodationDTO(Model.Accommodation accommodation)
        {
            var accommodationDTO = new DTO.AccommodationDTO()
            {
                Id = accommodation.Id,
                Name = accommodation.Name,
                StartSeason = accommodation.StartSeasonDate.ToString("yyyy-MM-dd"),
                EndSeason = accommodation.EndSeasonDate.ToString("yyyy-MM-dd"),
                Price = accommodation.AccomodationPrice.FinalPrice,
                PricePerGuest = accommodation.AccomodationPrice.PricePerGuest,
                PricePerAccomodation = accommodation.AccomodationPrice.PricePerAccomodation,
                HolidayCost = accommodation.AccomodationPrice.HolidayCost,
                WeekendCost = accommodation.AccomodationPrice.WeekendCost,
                SummerCost = accommodation.AccomodationPrice.SummerCost,
                HostId = accommodation.HostId,
                HostUsername = accommodation.HostUsername

            };

            return accommodationDTO;
        }


        public static Model.Accommodation AccommodationDTOToObject(DTO.AccommodationDTO dto)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("sr-Cyrl-Rs");
            var accommodation = new Model.Accommodation()
            {
                Name = dto.Name,
                StartSeasonDate = DateTime.ParseExact(dto.StartSeason, "yyyy-MM-dd", culture),
                EndSeasonDate = DateTime.ParseExact(dto.EndSeason, "yyyy-MM-dd", culture),
                AccomodationPrice = new Model.Price()
                {
                    FinalPrice = dto.Price,

                }


            };

            return accommodation;
        }


        public static AccommodationDTO ObjectToAccommodationDTOForSearch(Model.Accommodation accommodation)
        {
            AccommodationDTO accommodationDto = new AccommodationDTO();
            accommodationDto.Id = accommodation.Id;
            accommodationDto.Name = accommodation.Name;
            accommodationDto.StartSeason = accommodation.StartSeasonDate.ToString("yyyy-MM-dd");
            accommodationDto.EndSeason = accommodation.EndSeasonDate.ToString("yyyy-MM-dd");
            accommodationDto.Price = accommodation.AccomodationPrice.FinalPrice;
            accommodationDto.PricePerGuest = accommodation.AccomodationPrice.PricePerGuest;
            accommodationDto.PricePerAccomodation = accommodation.AccomodationPrice.PricePerAccomodation;
            accommodationDto.HolidayCost = accommodation.AccomodationPrice.HolidayCost;
            accommodationDto.WeekendCost = accommodation.AccomodationPrice.WeekendCost;
            accommodationDto.SummerCost = accommodation.AccomodationPrice.SummerCost;
            accommodationDto.Offers = accommodation.Offers;
            accommodationDto.IsDistinguishedHost = accommodation.IsDistinguishedHost;
            accommodationDto.HostId = accommodation.HostId;
            accommodationDto.AccommodationLocation = accommodation.Address.Street + ',' + accommodation.Address.Number + ',' + accommodation.Address.City + ',' + accommodation.Address.Country;

            return accommodationDto;

        }

    }

}
