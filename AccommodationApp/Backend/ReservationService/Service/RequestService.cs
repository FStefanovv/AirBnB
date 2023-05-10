using Microsoft.Extensions.Primitives;
using ReservationService.Model;
using ReservationService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReservationService.DTO;
using ReservationService.Enums;

namespace ReservationService.Service
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _repository;

        public RequestService(IRequestRepository repository)
        {
            _repository = repository;
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

        //to be called from UserService via gRPC to update status of all pending user requests to cancelled
        public void UpdateRequestsPostUserDeletion(string id)
        {
            _repository.UpdateRequestsPostUserDeletion(id);
        }

        public List<ReservationRequest> GetPendingRequestsByHost(StringValues userId)
        {
            return _repository.GetPendingRequestsByHost(userId);
        }

        public List<ReservationRequest> GetResolvedRequestsByHost(StringValues userId)
        {
            return _repository.GetResolvedRequestsByHost(userId);
        }

        public void CreateReserervationRequest(RequestReservationDTO dto)
        {
            ReservationRequest resRequest = Adapter.ReservationAdapter.RequestReservationDtoToReservationRequest(dto);
            _repository.Create(resRequest);
        }

        public void AcceptRequest(string requestId,string accommodationId)
        {
            AcceptRequestDatabaseUpdate(requestId);
            
        }

        private void AcceptRequestDatabaseUpdate(string requestId)
        {
            ReservationRequest reservationRequest = _repository.GetRequestById(requestId);
            reservationRequest.Status = RequestStatus.ACCEPTED;
            _repository.UpdateRequest(reservationRequest);
        }
    }
}
