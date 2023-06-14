using Microsoft.Extensions.Logging;
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

namespace ReservationService.Service
{
    public class ReservationService : ReservationGRPCService.ReservationGRPCServiceBase, IReservationService
    {
        private readonly IReservationRepository _repository;

        private readonly IRequestRepository _requestRepository;

        private readonly ILogger<ReservationService> _logger;
        private readonly string _url = "http://localhost:5002/Services.AccomodationService/GetAccommodationGRPC";


        public ReservationService(IReservationRepository repository, ILogger<ReservationService> logger, IRequestRepository requestRepository)
        {
            _repository = repository;
            _logger = logger;
            _requestRepository = requestRepository;
        }

        public override Task<Updated> UpdateRequestsPostUserDeletion(UserData userData, ServerCallContext context)
        {
            _requestRepository.UpdateRequestsPostUserDeletion(userData.Id);
            return Task.FromResult(new Updated
            {
                IsUpdated = true
            });
        }

        public void CancelReservation(string reservationId, StringValues userId)
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
                throw new Exception();
            else
            {
                reservation.Status = Enums.ReservationStatus.CANCELLED;
                _repository.UpdateReservation(reservation);
                //notify accommodation service that reservation slot is free
            }
        }

        public List<Reservation> GetUserReservations(StringValues userId)
        {
            return _repository.GetUserReservations(userId);
        }


        //to be called from UserService via gRPC to check whether user account can be deleted or not
        public override Task<ActiveReservation> GuestHasActiveReservations(UserData userData, ServerCallContext context)

        {
            List<Reservation> activeReservations = _repository.GetActiveUserReservations(userData.Id);

            return Task.FromResult(new ActiveReservation
            {
                IsReservationActive = (activeReservations.Count != 0)
            });


        }




        public override Task<ActiveReservation> HostHasActiveReservations(UserData userData, ServerCallContext context)
        {
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
                    startSummer[i] = new DateTime(2023 + i, 06, 22);
                    endSummer[i] = new DateTime(2023 + i, 09, 23);
                }

                for (int i = 0; i < startSummer.Count; i++)
                {

                    for (int j = 0; j < numberOfDays; i++)
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
            using var channel = GrpcChannel.ForAddress("https://localhost:5002",
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

        public void CreateReservationFromRequest(ReservationRequest request)
        {
            Reservation reservation = Adapter.ReservationAdapter.RequestToReservation(request);
            _repository.Create(reservation);
        }

        //         public async void CreateReservationGRPC(Reservation reservation)
        //         {
        //             AppContext.SetSwitch(
        //                 "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
        //             using var channel = GrpcChannel.ForAddress(_url);
        //             var client = new AccommodationGRPCService.AccommodationGRPCServiceClient(channel);

        //             var reply = await client.GetAccommodationGRPCAsync(new AccommodationId
        //             {
        //                 Id = "64487697c915d0ae735042a6"
        //             });
        //             _logger.LogInformation("Greeting: {reply.Name} -- {DateTime.Now}");
        //         }

        public override Task<CanRate> CheckIfUserCanRate(RatingData ratingData, ServerCallContext context)
        {
            bool userHasVisited = _repository.CheckIfUserHasUncancelledReservation(ratingData.UserId, ratingData.RatedEntityId);
            return Task.FromResult(new CanRate
            {
                RatingAllowed = userHasVisited
            });
        } 
        
        public void UpdatePastReservations()
        {
            _repository.UpdatePastReservations();
        }

        public override Task<HasReservation> AccommodatioHasReservation(AccId id, ServerCallContext context)
        {
            List<Reservation> reservationsPerAcc = _repository.GetReservationsForAccommodation(id.Id);
            return Task.FromResult(new HasReservation
            {
                Reservation = reservationsPerAcc.Count!=0
            });
        }
    }
}