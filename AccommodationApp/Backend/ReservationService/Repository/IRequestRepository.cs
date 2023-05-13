using Microsoft.Extensions.Primitives;
using ReservationService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationService.Repository
{
    public interface IRequestRepository
    {
        void UpdateRequest(ReservationRequest request);
        ReservationRequest GetRequestById(string requestId);
        List<ReservationRequest> GetRequestsByUser(string id);
        List<ReservationRequest> GetPendingRequestsByUser(string id);
        void UpdateRequestsPostUserDeletion(string id);
        List<ReservationRequest> GetPendingRequestsByHost(string userId);
        List<ReservationRequest> GetResolvedRequestsByHost(string userId);
        void Create(ReservationRequest resRequest);
        List<ReservationRequest> GetRequestsForCancelAfterAcceptingOne(string accommodationId);
    }
}
