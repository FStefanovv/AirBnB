using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using ReservationService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationService.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly PostgresDbContext _context;

        public ReservationRepository(PostgresDbContext context)
        {
            _context = context;
        }

        public ReservationRequest GetRequestById(string requestId)
        {
         
            return _context.Requests.Where(req => req.Id == requestId).FirstOrDefault();
        }

        public Reservation GetReservationById(string reservationId)
        {
            return _context.Reservations.Where(res => res.Id == reservationId).FirstOrDefault();
        }

        public List<Reservation> GetUserReservations(StringValues userId)
        {
            return _context.Reservations.Where(res => res.UserId == userId[0]).ToList();
        }

        public void UpdateRequest(ReservationRequest request)
        {
            _context.Entry(request).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
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
    }
}
