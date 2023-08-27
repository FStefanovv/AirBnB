using Microsoft.Extensions.Primitives;
using ReservationService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReservationService.DTO;
using Grpc.Core;

namespace ReservationService.Service
{
    public interface IReservationService
    {
        Task CancelReservation(string reservationId, StringValues userId, StringValues username);
        Task<double> GetCost(ReservationCostDTO reservation);
        List<ShowReservationDTO> GetUserReservations(StringValues userId);
        //bool GuestHasActiveReservations(string id);
        //bool HostHasActiveReservations(string id);
        //void CreateReservation(Reservation reservation, DTO.AccommodationDTO accommodation);
        List<DateTime> GetStartReservationDate(string accommodationId);
        List<DateTime> GetEndReservationDate(string accommodationId);
        void UpdatePastReservations();
        Task CreateReservationFromRequest(ReservationRequest request);
        List<GetBusyDateForAccommodationDTO> GetBusyDatesForAccommodation(string accommodationId);
        ShowReservationDTO GetEndReservation(string id);
        Task<IsAvailable> CheckIfAccommodationIsAvailable(AvailabilityPeriod availabilityPeriod, ServerCallContext context);
        Task<bool> CheckHostStatus(String hostId);
        Task<bool> UpdateReservationStatus(String id, bool IsSatisfied);
        bool HostHasActiveReservationsSaga(string hostId);
        Task Create(Reservation reservation);
    }
}
