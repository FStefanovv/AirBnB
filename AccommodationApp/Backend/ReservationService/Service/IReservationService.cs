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
        Task<double> GetCost(ReservationCostDTO reservation);
        List<DateTime> GetStartReservationDate(string accommodationId);
        List<DateTime> GetEndReservationDate(string accommodationId);
        void UpdatePastReservations();
        void CreateReservationFromRequest(ReservationRequest request);
        List<GetBusyDateForAccommodationDTO> GetBusyDatesForAccommodation(string accommodationId);
        Task<bool> CheckHostStatus(String hostId);
    }
}
