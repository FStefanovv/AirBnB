using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using ReservationService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReservationService.DTO;

namespace ReservationService.Repository
{
    public class RequestRepository : IRequestRepository
    {
        private readonly PostgresDbContext _context;

        public RequestRepository(PostgresDbContext context)
        {
            _context = context;
        }

        public void Create(ReservationRequest resRequest)
        {
            _context.Requests.Add(resRequest);
            _context.SaveChanges();
         
        }
        
        public ReservationRequest GetRequestById(string requestId)
        {

            return _context.Requests.Where(req => req.Id == requestId).FirstOrDefault();
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

        public List<ReservationRequest> GetRequestsByUser(string id)
        {
            return _context.Requests.Where(req => req.UserId == id).ToList();
        }

        public List<ReservationRequest> GetPendingRequestsByUser(string id)
        {
            return _context.Requests.Where(req => req.UserId == id && req.Status == Enums.RequestStatus.PENDING).ToList();
        }

        public void UpdateRequestsPostUserDeletion(string id)
        {
            List<ReservationRequest> requests = GetPendingRequestsByUser(id);

            foreach(ReservationRequest req in requests)
            {
                req.Status = Enums.RequestStatus.USER_DELETED;
                _context.Entry(req).State = EntityState.Modified;
            }

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public List<ReservationRequest> GetPendingRequestsByHost(string userId)
        {
            return _context.Requests.Where(req => req.HostId == userId && req.Status==Enums.RequestStatus.PENDING).ToList();
        }

        public List<ReservationRequest> GetResolvedRequestsByHost(string userId)
        {
            return _context.Requests.Where(req => req.HostId == userId && req.Status != Enums.RequestStatus.PENDING).ToList();
        }
        
        public List<ReservationRequest> GetRequestsForCancelAfterAcceptingOne(string accommodationId)
        {
            return _context.Requests.Where(req => req.AccommodationId == accommodationId && req.Status == Enums.RequestStatus.PENDING).ToList();
        }

        public List<ReservationRequest> GetRequestsForHost(string hostId)
        {
            return _context.Requests.Where(req => req.HostId == hostId).ToList();
        }
    }
}
