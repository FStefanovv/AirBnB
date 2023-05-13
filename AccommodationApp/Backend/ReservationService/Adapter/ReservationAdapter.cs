using ReservationService.DTO;
using ReservationService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReservationService.Enums;

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

        private DatesAdapter datesAdapter = new DatesAdapter();

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

        public static ReservationRequest RequestReservationDtoToReservationRequest(RequestReservationDTO dto)
        {
            ReservationRequest resRequest = new ReservationRequest();
            resRequest.HostId = dto.HostId;
            resRequest.UserId = dto.UserId;
            resRequest.AccommodationId = dto.AccomodationId;
            resRequest.NumberOfGuests = dto.NumberOfGuests;
            resRequest.From = Convert.ToDateTime(dto.StartDate);
            resRequest.To = Convert.ToDateTime(dto.EndDate);
            resRequest.Status = RequestStatus.PENDING;
            return resRequest;
        }

        public static GetBusyDateForAccommodationDTO ReservationToGetBusyDateForAccommodationDTO(
            Reservation reservation)
        {
            GetBusyDateForAccommodationDTO dto = new GetBusyDateForAccommodationDTO();
            dto.AccommodationId = reservation.AccommodationId;
            dto.From = reservation.From.ToString();
            dto.To = reservation.To.ToString();
            return dto;
        }
    }
}
