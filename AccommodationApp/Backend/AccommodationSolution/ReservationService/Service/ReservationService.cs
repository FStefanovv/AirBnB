﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using ReservationService.Model;
using ReservationService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReservationService.DTO;
using Grpc.Core;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Grpc.Net.Client;
using System.Net.Http;
using Users;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;
using Jaeger;
using MassTransit;
using ReservationService.RabbitMQ;
using NotificationsService.RabbitMQ;

namespace ReservationService.Service
{
    public class ReservationService : ReservationGRPCService.ReservationGRPCServiceBase, IReservationService
    {
        private readonly IReservationRepository _repository;

        private readonly IRequestRepository _requestRepository;

        private readonly ILogger<ReservationService> _logger;
        private readonly string _url = "http://localhost:5002/Services.AccomodationService/GetAccommodationGRPC";
        private readonly ITracer _tracer;

        private readonly ISendEndpointProvider _sendEndpointProvider;



        public ReservationService(IReservationRepository repository, ILogger<ReservationService> logger, IRequestRepository requestRepository,ITracer tracer, ISendEndpointProvider sendEndpointProvider)
        {
            _repository = repository;
            _logger = logger;
            _requestRepository = requestRepository;
            _tracer = tracer;
            _sendEndpointProvider = sendEndpointProvider;
        }

        public override Task<Updated> UpdateRequestsPostUserDeletion(UserData userData, ServerCallContext context)
        {
            using var scope = _tracer.BuildSpan("UpdateRequestsPostUserDeletion").StartActive(true);
            _requestRepository.UpdateRequestsPostUserDeletion(userData.Id);
            return Task.FromResult(new Updated
            {
                IsUpdated = true
            });
        }

        public async Task Create(Reservation reservation)
        {
            _repository.Create(reservation);
        }

        public async Task CancelReservation(string reservationId, StringValues userId, StringValues username)
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
                throw new Exception("You cannot cancel a reservation less than 24 hours prior to it's beginning.");
            else
            {
                reservation.Status = Enums.ReservationStatus.CANCELLED;
                _repository.UpdateReservation(reservation);
                await SendNotification(reservation.HostId, username + " has cancelled a reservation for " + reservation.AccommodationName);
                await CheckHostStatus(reservation.HostId);
            }
        }

        private async Task SendNotification(string userId, string notificationContent)
        {
            var endPoint = await _sendEndpointProvider.
                GetSendEndpoint(new Uri("queue:" + BusConstants.NotificationQueue));
            await endPoint.Send<INotification>(new INotification { UserId = userId, NotificationContent = notificationContent, CreatedAt = DateTime.Now });
        }


        public List<ShowReservationDTO> GetUserReservations(StringValues userId)
        {
            return _repository.GetUserReservations(userId);
        }


        public override Task<ActiveReservation> GuestHasActiveReservations(UserData userData, ServerCallContext context)

        {
            using var scope = _tracer.BuildSpan("GuestHasActiveReservations").StartActive(true);
            List<Reservation> activeReservations = _repository.GetActiveUserReservations(userData.Id);

            return Task.FromResult(new ActiveReservation
            {
                IsReservationActive = (activeReservations.Count != 0)
            });


        }
        public bool HostHasActiveReservationsSaga(string hostId)
        {
            List<Reservation> activeReservations = _repository.GetActiveHostReservations(hostId);



            return (activeReservations.Count == 0);
           
        }



        public override Task<ActiveReservation> HostHasActiveReservations(UserData userData, ServerCallContext context)
        {
            using var scope = _tracer.BuildSpan("HostHasActiveReservations").StartActive(true);
            List<Reservation> activeReservations = _repository.GetActiveHostReservations(userData.Id);


            return Task.FromResult(new ActiveReservation
            {
                IsReservationActive = (activeReservations.Count != 0)
            });
        }


        public async Task<double> GetCost(ReservationCostDTO reservation)
        {

            AccommodationGRPC accommodation = await getAccommodation(reservation);
            int numberOfDays = (reservation.To - reservation.From).Days;
            int weekends = 0;
            int holidays = 0;
            int summerDays = 0;
            double price = 0;

            if (accommodation.WeekendCost == false && accommodation.HolidayCost == false && accommodation.SummerCost == false)
            {
                if (accommodation.PricePerAccomodation)
                {
                    return price = accommodation.Price * (numberOfDays - 1);

                }
                else if (accommodation.PricePerGuest)
                {
                    return price = (accommodation.Price * reservation.NumberOfGuests) * (numberOfDays - 1);

                }
            }



            if (accommodation.WeekendCost)
            {

                for (int i = 0; i < numberOfDays; i++)
                {
                    if (reservation.From.AddDays(i).DayOfWeek == DayOfWeek.Saturday)
                    { weekends++; }
                    if (reservation.From.AddDays(i).DayOfWeek == DayOfWeek.Sunday)
                    { weekends++; }
                }
                Console.WriteLine("Number of weekends " + weekends);

            }


            if (accommodation.HolidayCost)
            {

                var listHolidays = new List<DateTime>
                {
                  new DateTime(2023, 12, 31),
                  new DateTime(2024, 01, 06),
                  new DateTime(2024, 02, 01),
                };

                foreach (var holiday in listHolidays)
                {
                    for (int i = 0; i < numberOfDays; i++)
                    {
                        if (reservation.From.AddDays(i) == holiday)
                        {
                            holidays++;
                        }
                    }
                }

                Console.WriteLine("Number of holidays " + holidays);

            }


            if (accommodation.SummerCost)
            {
                var startSummer = new List<DateTime> { };
                var endSummer = new List<DateTime> { };
                for (int i = 0; i < 100; i++)
                {
                    startSummer.Add(new DateTime(2023 + i, 06, 22));
                    endSummer.Add(new DateTime(2023 + i, 09, 23));
                }

                for (int i = 0; i < startSummer.Count; i++)
                {

                    for (int j = 0; j < numberOfDays; j++)
                    {
                        if (reservation.From.AddDays(j) >= startSummer[i] && reservation.From.AddDays(j) <= endSummer[i])
                        {
                            summerDays++;
                        }
                    }

                }


                Console.WriteLine("Number of summmer days " + summerDays);



            }

            if (accommodation.WeekendCost == true || accommodation.HolidayCost == true || accommodation.SummerCost || true)
            {



                if (accommodation.PricePerAccomodation)
                {
                    return price = accommodation.Price * (numberOfDays - 1) + (accommodation.Price * 0.2) * (weekends + holidays + summerDays);

                }
                else if (accommodation.PricePerGuest)
                {
                    return price = (accommodation.Price * reservation.NumberOfGuests) * (numberOfDays - 1) + (accommodation.Price * 0.2 * reservation.NumberOfGuests) * (weekends + holidays + summerDays);

                }
            }
            return price;
        }

        public async Task<AccommodationGRPC> getAccommodation(ReservationCostDTO dto)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            using var channel = GrpcChannel.ForAddress("https://accommodation-service:443",
                new GrpcChannelOptions { HttpHandler = handler });
            var client = new AccommodationGRPCService.AccommodationGRPCServiceClient(channel);
            var reply = await client.GetAccommodationGRPCAsync(new AccommodationId
            {
                Id = dto.AccommodationId
            });

            return reply;
        }

        public List<DateTime> GetStartReservationDate(string accommodationId)
        {
            List<Reservation> reservations = _repository.GetReservationsForAccommodation(accommodationId);
            List<DateTime> startDates = new List<DateTime>();
            int i = 0;

            foreach (Reservation reservation in reservations)
            {
                
                startDates[i] = reservation.From;
                i++;
            }

            return startDates;
        }

        public List<DateTime> GetEndReservationDate(string accommodationId)
        {

            List<Reservation> reservations = _repository.GetReservationsForAccommodation(accommodationId);
            List<DateTime> endDates = new List<DateTime>();
            int i = 0;

            foreach (Reservation reservation in reservations)
            {

                endDates[i] = reservation.To.AddDays(-1);
                i++;
            }

            return endDates;
        }

        public List<GetBusyDateForAccommodationDTO> GetBusyDatesForAccommodation(string accommodationId)
        {

        //    AppContext.SetSwitch(
        //        "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
        //    using var channel = GrpcChannel.ForAddress(_url);
        //    var client = new AccommodationGRPCService.AccommodationGRPCServiceClient(channel);

        //    var reply = await client.GetAccommodationGRPCAsync(new AccommodationId
        //    {
        //        Id = "64487697c915d0ae735042a6"
        //    });
        //    _logger.LogInformation("Greeting: {reply.Name} -- {DateTime.Now}");

            List<GetBusyDateForAccommodationDTO> dtoList = new List<GetBusyDateForAccommodationDTO>();
            foreach (var reservation in _repository.GetReservationsForAccommodation(accommodationId))
            {
                dtoList.Add(Adapter.ReservationAdapter.ReservationToGetBusyDateForAccommodationDTO(reservation));
            }

            return dtoList;

        }

        public async Task CreateReservationFromRequest(ReservationRequest request)
        {
            ReservationCostDTO dto = new ReservationCostDTO();
            dto.From = request.From;
            dto.To = request.To;
            dto.NumberOfGuests = request.NumberOfGuests;
            dto.AccommodationId = request.AccommodationId;
            dto.AccommodationName = request.AccommodationName;
            Reservation reservation = Adapter.ReservationAdapter.RequestToReservation(request);
            AccommodationGRPC accommodation = await getAccommodation(dto);
            reservation.Price = await GetCost(dto);
            reservation.AccommodationLocaiton = accommodation.Address;
            _repository.Create(reservation);
        }

        public override Task<CanRate> CheckIfUserCanRate(RatingData ratingData, ServerCallContext context)
        {
            using var scope = _tracer.BuildSpan("CheckIfUserCanRate").StartActive(true);
            bool userHasVisited = _repository.CheckIfUserHasUncancelledReservation(ratingData.UserId, ratingData.RatedEntityId);
            return Task.FromResult(new CanRate
            {
                RatingAllowed = userHasVisited
            });
        } 

        public CanRateDTO CheckIfUserCanRate(StringValues userId, string hostId, string accommId)
        {
            bool canRateHost = _repository.CheckIfUserHasUncancelledReservation(userId, hostId);
            bool canRateAccomm = _repository.CheckIfUserHasUncancelledReservation(userId, accommId);

            return new CanRateDTO { Host = canRateHost, Accommodation = canRateAccomm };
        }
        
        public void UpdatePastReservations()
        {
            _repository.UpdatePastReservations();
        }

        public override Task<HasReservation> AccommodatioHasReservation(AccId id, ServerCallContext context)
        {
            using var scope = _tracer.BuildSpan("AccommodationHasReservation").StartActive(true);
            List<Reservation> reservationsPerAcc = _repository.GetReservationsForAccommodation(id.Id);
            return Task.FromResult(new HasReservation
            {
                Reservation = reservationsPerAcc.Count != 0
            });
        }
        public ShowReservationDTO GetEndReservation(string id)
        {
            Reservation res = _repository.GetReservationById(id);

            return new ShowReservationDTO {
                Id = res.Id,
                From = res.From,
                To = res.To,
                AccommodationName = res.AccommodationName,
                AccommodationLocation = res.AccommodationLocaiton,
                NumberOfGuests = res.NumberOfGuests,
                Status = res.Status,
                Price = res.Price
            };
        }

        public override Task<IsAvailable> CheckIfAccommodationIsAvailable(AvailabilityPeriod availabilityPeriod, ServerCallContext context)
        {
            using var scope = _tracer.BuildSpan("CheckingIfAccomodationsIsAvailable").StartActive(true);
            List<Reservation> AccomodationReservations = _repository.GetReservationsForAccommodation(availabilityPeriod.AccommodationId);
            if (AccomodationReservations.Count == 0)
            {
                return Task.FromResult(new IsAvailable
                {
                    Available = true
                });
            }
            else
            {
                foreach (Reservation reservation in AccomodationReservations)          
                {
                    if (reservation.Status == Enums.ReservationStatus.ACTIVE)
                    {
                        if (DateTime.Parse(availabilityPeriod.StartDate) <= reservation.From && DateTime.Parse(availabilityPeriod.EndDate) <= reservation.From)
                        {
                            return Task.FromResult(new IsAvailable
                            {
                                Available = true
                            });
                        }
                        else if (DateTime.Parse(availabilityPeriod.StartDate) >= reservation.To && DateTime.Parse(availabilityPeriod.EndDate) >= reservation.To)
                        {
                            return Task.FromResult(new IsAvailable
                            {
                                Available = true
                            });
                        }
                    }
                    else
                    {
                        return Task.FromResult(new IsAvailable
                        {
                            Available = true
                        });
                    }
                }

                return Task.FromResult(new IsAvailable
                {
                    Available = false
                });
            }
        }


        public async Task CheckHostStatus(String hostId)
        {
            List<Reservation> cancelledReservations = _repository.GetCanceledHostReservations(hostId);
            List<Reservation> allReservations = _repository.GetAllHostReservations(hostId);

            bool lowCancellationRate = ((double) cancelledReservations.Count / allReservations.Count) * 100 < 5;

            int numberOfPastReservations = allReservations.Where<Reservation>(res => res.Status==Enums.ReservationStatus.PAST).ToList().Count;

            if (lowCancellationRate && numberOfPastReservations >= 5)
            {
                TimeSpan TotalDuration = TimeSpan.Zero;

                foreach (Reservation reservation in allReservations)
                {
                    TimeSpan durationOfReservation = reservation.To - reservation.From;
                    TotalDuration += durationOfReservation;
                }

                if (TotalDuration.TotalDays > 50)
                {
                    await UpdateReservationStatus(hostId, true);
                }
                else
                {
                    await UpdateReservationStatus(hostId, false);
                }
            }
            else
            {
               await UpdateReservationStatus(hostId, false);
            }
        }



        public async Task<bool> UpdateReservationStatus(String id, bool IsSatisfied)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            using var channel = GrpcChannel.ForAddress("https://user-service:443",
                new GrpcChannelOptions { HttpHandler = handler });
            var client = new UserGRPCService.UserGRPCServiceClient(channel);
            var reply = await client.IsReservationPartSatisfiedAsync(new ReservationSatisfied
            {
                Id = id,
                ReservationPartSatisfied = IsSatisfied
            });
            return reply.IsReservationSatisfied;
        }
    }
}