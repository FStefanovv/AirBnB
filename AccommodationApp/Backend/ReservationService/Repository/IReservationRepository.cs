using Microsoft.Extensions.Primitives;
using ReservationService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationService.Repository
{
    public interface IReservationRepository
    {   
        void UpdateReservation(Reservation reservation);
        Reservation GetReservationById(string reservationId);
        List<Reservation> GetUserReservations(StringValues userId);
        List<Reservation> GetActiveUserReservations(string id);
        List<Reservation> GetActiveHostReservations(string id);
        List<Reservation> GetPastHostReservations (string id);
        List<Reservation> GetCanceledHostReservations(string id);
        List<Reservation> GetAllHostReservations(string id);
        void Create(Reservation reservation);
        List<Reservation> GetReservationsForAccommodation(string accomodationId);
        bool CheckIfUserHasUncancelledReservation(string userId, string ratedEntityId);
        List<Reservation> GetPastReservations();
        void UpdatePastReservations();
    }
}
