using Microsoft.Extensions.Primitives;
using ReservationService.Model;
using ReservationService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReservationService.DTO;
using ReservationService.Enums;
using ReservationService.Helpers;
using Grpc.Core;
using System.Net.Http;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;

namespace ReservationService.Service
{
    public class RequestService :  IRequestService
    {
        private readonly IRequestRepository _repository;
        private readonly IReservationService _reservation;
        private readonly ITracer _tracer;


        public RequestService(IRequestRepository repository,IReservationService reservation, ITracer tracer)
        {
            _repository = repository;
            _reservation = reservation;
            _tracer = tracer;
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

       // to be called from UserService via gRPC to update status of all pending user requests to cancelled
      

        public List<ReservationRequest> GetPendingRequestsByHost(StringValues userId)
        {
            return _repository.GetPendingRequestsByHost(userId);
        }

        public List<ReservationRequest> GetResolvedRequestsByHost(StringValues userId)
        {
            return _repository.GetResolvedRequestsByHost(userId);
        }

        public List<ReservationRequest> GetRequestsForUser(string userId)
        {
            return _repository.GetPendingRequestsByUser(userId);
        }

        public async Task<bool> CreateReservationRequestOrReservation(RequestReservationDTO dto)
        {
            bool autoApproval = await IsAutoApproval(dto.AccomodationId);
            
            if (autoApproval == true)
            {
                Reservation reservation = Adapter.ReservationAdapter.RequestReservationDtoToReservation(dto);
                await _reservation.CheckHostStatus(dto.HostId);
                await _reservation.Create(reservation);
            }
            else
            {
                ReservationRequest resRequest = Adapter.ReservationAdapter.RequestReservationDtoToReservationRequest(dto);
                _repository.Create(resRequest);
                await SendNotification(dto.HostId, dto.UserId + " has created a reservation request for " + dto.AccommodationName);

            }
            return true;
        }

        private async Task SendNotification(string hostId, string notificationContent)
        {
            using var scope = _tracer.BuildSpan("SendCancellationNotificaiton").StartActive(true);
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            using var channel = GrpcChannel.ForAddress("https://notification-service:443",
                new GrpcChannelOptions { HttpHandler = handler });
            var client = new NotificationGRPCService.NotificationGRPCServiceClient(channel);
            var reply = await client.CreateNotificationAsync(new NotificationData
            {
                UserId = hostId,
                NotificationContent = notificationContent
            });

        }

        public async Task<bool> IsAutoApproval(StringValues id)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            using var channel = GrpcChannel.ForAddress("https://accommodation-service:443",
                new GrpcChannelOptions { HttpHandler = handler });
            var client = new AccommodationGRPCService.AccommodationGRPCServiceClient(channel);
            var reply = await client.GetAccomodationsAutoApprovalAsync(new AccommodationIdForApproval
            {
                AccId = id
            });
            return reply.AutoApproval;

        }

        public async Task AcceptRequest(string requestId,string accommodationId)
        {
            ReservationRequest request = _repository.GetRequestById(requestId);
            AcceptRequestDatabaseUpdate(request);
            //fali grpc sa accomodationom
            await _reservation.CreateReservationFromRequest(request);
            CancelRequestsInTimeRange(accommodationId, request.From, request.To);
            await _reservation.CheckHostStatus(request.HostId);
            await SendNotification(request.UserId, "Your request for " + request.AccommodationName + " has been accepted!");

        }

        private void AcceptRequestDatabaseUpdate(ReservationRequest request)
        {
            request.Status = RequestStatus.ACCEPTED;
            _repository.UpdateRequest(request);
        }

        private void CancelRequestsInTimeRange(string accommodationId, DateTime startDate, DateTime endDate)
        {
            List<ReservationRequest> requestList = _repository.GetRequestsForCancelAfterAcceptingOne(accommodationId);
            foreach (var request in requestList)
            {
                    if (startDate.InRange(request.From, request.To) || endDate.InRange(request.From, request.To))
                    {
                        request.Status = RequestStatus.DENIED;
                        _repository.UpdateRequest(request);
                    }
            }
        }

        public List<ShowRequestDTO> GetRequestsForHost(string hostId)
        {
            List<ShowRequestDTO> dtoList = new List<ShowRequestDTO>();
            foreach (var request in _repository.GetRequestsForHost(hostId))
            {
                dtoList.Add(Adapter.ReservationAdapter.ReservationRequestToShowRequestDto(request));
            }

            return dtoList;
        }

        public List<ShowRequestDTO> GetRequestsForUsers(string userId)
        {
            List<ShowRequestDTO> dtoList = new List<ShowRequestDTO>();
            foreach (var request in _repository.GetPendingRequestsByUser(userId))
            {
                dtoList.Add(Adapter.ReservationAdapter.ReservationRequestToShowRequestDto(request));
            }

            return dtoList;
        }
    }
}
