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
