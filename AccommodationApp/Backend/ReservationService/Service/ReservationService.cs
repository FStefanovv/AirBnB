using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using ReservationService.Model;
using ReservationService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationService.Service
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _repository;

        private readonly ILogger<ReservationService> _logger;
        private readonly string _url = "http://localhost:5002";


        public ReservationService(IReservationRepository repository, ILogger<ReservationService> logger)
        {
            _repository = repository;
            _logger = logger;
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
        public bool GuestHasActiveReservations(string id)
        {
            List<Reservation> activeReservations = _repository.GetActiveUserReservations(id);


            return activeReservations.Count != 0;
        }

        public bool HostHasActiveReservations(string id)
        {
            List<Reservation> activeReservations = _repository.GetActiveHostReservations(id);


            return activeReservations.Count != 0;
        }


        public void CreateReservation(Reservation reservation, DTO.AccommodationDTO accommodation)
        {
            int numberOfDays = (reservation.To - reservation.From).Days;
            int weekends = 0;
            int holidays = 0;
            int summerDays = 0;

            if (accommodation.WeekendCost == false && accommodation.HolidayCost == false && accommodation.SummerCost == false)
            {
                if (accommodation.PricePerAccomodation)
                {
                    reservation.Price = accommodation.Price * (numberOfDays - 1);
                    _repository.Create(reservation);
                }
                else if (accommodation.PricePerGuest)
                {
                    reservation.Price = (accommodation.Price * reservation.NumberOfGuests) * (numberOfDays - 1);
                    _repository.Create(reservation);
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
                    reservation.Price = accommodation.Price * (numberOfDays - 1) + (accommodation.Price * 0.2) * (weekends+holidays+summerDays);
                    _repository.Create(reservation);
                }
                else if (accommodation.PricePerGuest)
                {
                    reservation.Price = (accommodation.Price * reservation.NumberOfGuests) * (numberOfDays - 1) + (accommodation.Price * 0.2 * reservation.NumberOfGuests) * (weekends + holidays + summerDays);
                    _repository.Create(reservation);
                }
            }
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


        public void CreateReservationGRPC(Reservation reservation)
        {
            using var channel = GrpcChannel.ForAddress(_url);
            var client = new AccommodationGRPCService.AccommodationGRPCServiceClient(channel);


            var reply = client.GetAccommodationGRPC(new AccommodationId
            {
                Id = "64345c35782e3689729e953b"
            });
            _logger.LogInformation("Greeting: {reply.Name} -- {DateTime.Now}");
        }
    }
}