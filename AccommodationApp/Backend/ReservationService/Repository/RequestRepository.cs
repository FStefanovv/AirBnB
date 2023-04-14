using Microsoft.EntityFrameworkCore;
using ReservationService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationService.Repository
{
    public class RequestRepository : IRequestRepository
    {
        private readonly PostgresDbContext _context;

        public RequestRepository(PostgresDbContext context)
        {
            _context = context;
        }

        public ReservationRequest GetById(string id)
        {
            /*
            foreach(ReservationRequest request in _context.Requests)
            {
                if (request.Id == id)
                    return request;
            }
            return null; */
            return _context.Requests.Where(req => req.Id == id).FirstOrDefault();
        }

        public void Update(ReservationRequest request)
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
    }
}
