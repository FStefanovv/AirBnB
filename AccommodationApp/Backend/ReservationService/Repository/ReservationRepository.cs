using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using ReservationService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReservationService.DTO;
using ReservationService.Enums;

namespace ReservationService.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly PostgresDbContext _context;

        public ReservationRepository(PostgresDbContext context)
        {
            _context = context;
        }

        public Reservation GetReservationById(string reservationId)
        {
            return _context.Reservations.Where(res => res.Id == reservationId).FirstOrDefault();
        }

        public List<Reservation> GetUserReservations(StringValues userId)
        {
            return _context.Reservations.Where(res => res.UserId == userId[0]).ToList();
        }

        public void UpdateReservation(Reservation reservation)
        {
            _context.Entry(reservation).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public List<Reservation> GetActiveUserReservations(string id)
        {
            return _context.Reservations.Where(res => res.UserId == id && res.Status==Enums.ReservationStatus.ACTIVE).ToList();
        }

        public List<Reservation> GetActiveHostReservations(string id)
        {
            return _context.Reservations.Where(res => res.HostId == id && res.Status == Enums.ReservationStatus.ACTIVE).ToList();
        }

        public List<Reservation> GetPastHostReservations(string id)
        {
            return _context.Reservations.Where(res => res.HostId == id && res.Status == ReservationStatus.PAST).ToList();
        }

        public List<Reservation> GetCanceledHostReservations(string id)
        {
            return _context.Reservations.Where(res => res.HostId == id && res.Status == ReservationStatus.CANCELLED).ToList();
        }

        public List<Reservation> GetAllHostReservations(string id)
        {
            return _context.Reservations.Where(res => res.HostId==id).ToList();
        }

        public void Create(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
        }


        public List<Reservation>  GetReservationsForAccommodation (string accomodationId)
        {
           return  _context.Reservations.Where(res=> res.AccommodationId == accomodationId).ToList();
        }

        public bool CheckIfUserHasUncancelledReservation(string userId, string ratedEntityId)
        {
            Reservation userReservation = _context.Reservations.Where(res => res.UserId == userId && (res.HostId==ratedEntityId || res.AccommodationId == ratedEntityId) && res.Status == ReservationStatus.PAST).FirstOrDefault();

            return userReservation != null;
        }

        public List<Reservation> GetPastReservations()
        {
            return _context.Reservations.Where(res => res.To < DateTime.Now && res.Status==Enums.ReservationStatus.ACTIVE).ToList();
        }

       

        public void UpdatePastReservations()
        {
            List<Reservation> pastReservations = GetPastReservations();
            foreach(Reservation res in pastReservations)
            {
                res.Status = Enums.ReservationStatus.PAST;
            }
            _context.SaveChanges();
        }
    }
}
