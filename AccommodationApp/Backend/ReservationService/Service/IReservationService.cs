using Microsoft.Extensions.Primitives;
using ReservationService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationService.Service
{
    public interface IReservationService
    {
        void CancelReservationRequest(string requestId, StringValues userId);
        void CancelReservation(string reservationId, StringValues userId);
        List<Reservation> GetUserReservations(StringValues userId);
    }
}
