using Microsoft.Extensions.Primitives;
using ReservationService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReservationService.DTO;

namespace ReservationService.Service
{
    public interface IReservationService
    {
        void CancelReservation(string reservationId, StringValues userId);
        List<Reservation> GetUserReservations(StringValues userId);
        bool GuestHasActiveReservations(string id);
        bool HostHasActiveReservations(string id);
        void CreateReservation(Reservation reservation, DTO.AccommodationDTO accommodation);
        List<DateTime> GetStartReservationDate(string accommodationId);
        List<DateTime> GetEndReservationDate(string accommodationId;
        void CreateReservationFromRequest(ReservationRequest request);
        List<GetBusyDateForAccommodationDTO> GetBusyDatesForAccommodation(string accommodationId);
    }
}
