using Microsoft.Extensions.Primitives;
using ReservationService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReservationService.DTO;

namespace ReservationService.Service
{
    public interface IRequestService
    {
        void CancelReservationRequest(string requestId, StringValues userId);
        void UpdateRequestsPostUserDeletion(string id);
        List<ReservationRequest> GetPendingRequestsByHost(StringValues userId);
        List<ReservationRequest> GetResolvedRequestsByHost(StringValues userId);
        void CreateReserervationRequest(RequestReservationDTO dto);
    }
}
