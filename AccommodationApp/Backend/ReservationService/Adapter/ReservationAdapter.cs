using ReservationService.DTO;
using ReservationService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationService.Adapter
{
    public class ReservationAdapter
    {
        /*
        public List<ReservationDTO> ReservationsToDto(List<Reservation> reservations)
        {
            List<ReservationDTO> dtos = new List<Reservation>();
            
            foreach(Reservation r in reservations)
            {
                ReservationDTO dto = ReservationToDto();
            }

            return dtos;
        }*/


        public static Reservation CreateReservationDtoToObject(ReservationDTO dto,string userId)
        {

            var reservation = new Reservation()
            {
                From = dto.From,
                To = dto.To,
                UserId = userId,
                AccommodationId = dto.AccommodationId,
                AccommodationName = dto.AccommodationName,
                NumberOfGuests = dto.NumberOfGuests,
                Status = Enums.ReservationStatus.ACTIVE,
                Price = 0
            };


            return reservation;
        
                

        }
    }
}
