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
        void UpdateRequest(ReservationRequest request);
        void UpdateReservation(Reservation reservation);

        ReservationRequest GetRequestById(string requestId);
        Reservation GetReservationById(string reservationId);
        List<Reservation> GetUserReservations(StringValues userId);
    }
}
