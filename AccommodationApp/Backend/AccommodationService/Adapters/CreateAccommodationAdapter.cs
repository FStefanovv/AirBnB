using Accommodation.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accommodation.Adapters
{
    public static class CreateAccommodationAdapter
    {
        public static Model.Accommodation CreateAccommodaitonDtoToObject(CreateAccommodationDTO dto, string hostUsername)
        {
            var accommodation = new Model.Accommodation()
            {
                Name = dto.Name,
                Address = dto.Address,
                Offers = dto.Offers,
                MaxGuests = dto.MaxGuests,
                MinGuests = dto.MinGuests,
                Host = hostUsername
            };

            return accommodation;
        }

    }
}
