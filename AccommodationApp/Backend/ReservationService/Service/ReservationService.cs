using Microsoft.Extensions.Primitives;
using ReservationService.Model;
using ReservationService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationService.Service
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _repository;

        public ReservationService(IReservationRepository repository)
        {
            _repository = repository;
        }

        public void CancelReservation(string reservationId, StringValues userId)
        {
            Reservation reservation = _repository.GetReservationById(reservationId);
            if (reservation == null)
                throw new Exception();
            else if (reservation.UserId != userId)
                throw new Exception();
            else if (reservation.Status != Enums.ReservationStatus.ACTIVE)
                throw new Exception();

            int differenceInDays = (reservation.From - DateTime.Now).Days;

            if (differenceInDays < 1)
                throw new Exception();
            else
            {
                reservation.Status = Enums.ReservationStatus.CANCELLED;
                _repository.UpdateReservation(reservation);
                //notify accommodation service that reservation slot is free
            }
        }

        public void CancelReservationRequest(string requestId, StringValues userId)
        {
            ReservationRequest request = _repository.GetRequestById(requestId);
            if (request == null)
                throw new Exception();
            else if (request.UserId != userId)
                throw new Exception();
            else if (request.Status != Enums.RequestStatus.PENDING)
                throw new Exception();
            else
            {
                request.Status = Enums.RequestStatus.CANCELLED;
                _repository.UpdateRequest(request);
            }

        }

        public List<Reservation> GetUserReservations(StringValues userId)
        {
            return _repository.GetUserReservations(userId);
        }
    }
}
