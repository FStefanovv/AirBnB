﻿using Accommodation.Adapters;
using Accommodation.DTO;
using Accommodation.Repository;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Accommodation.Model;
using System.Net.Sockets;
using System.Xml.Linq;
using MongoDB.Driver.Linq;
using System.Text.RegularExpressions;
using System.Globalization;
using Grpc.Net.Client;
using System.Net.Http;
using OpenTracing;
using Jaeger;

namespace Accommodation.Services
{
    public class AccommodationService : AccommodationGRPCService.AccommodationGRPCServiceBase
    {
        private readonly Repository.AccommodationRepository _repository;
        private readonly ITracer _tracer;

        public AccommodationService(Repository.AccommodationRepository repository, ITracer tracer)
        {
            _repository = repository;
            _tracer = tracer;
        }

        public void Create(DTO.CreateAccommodationDTO dto, StringValues hostId, StringValues hostUsername, List<IFormFile> photos)
        {
            Model.Accommodation accommodation = Adapters.CreateAccommodationAdapter.CreateAccommodaitonDtoToObject(dto, hostId, hostUsername);

            _repository.Create(accommodation, photos);
        }

        public async Task<List<byte[]>> GetAccommodationPhotos(string accommId)
        {
            return await _repository.GetAccommodationPhotos(accommId);
        }

        public override Task<Deleted>   DeleteAccwithoutHost(UserId userId, ServerCallContext context)
        {
            using var scope = _tracer.BuildSpan("DeleteAccwithoutHost").StartActive(true);
            _repository.DeleteAccWithoutHost(userId.Id);
            return Task.FromResult(new Deleted
            {
                IsDeleted = true
            });
        }

        public override Task<AccommodationAutoApproval> GetAccomodationsAutoApproval(AccommodationIdForApproval accommodationId, ServerCallContext context)
        {
            using var scope = _tracer.BuildSpan("AccommodationIdForApproval").StartActive(true);
            Model.Accommodation accommodation = _repository.GetById(accommodationId.AccId);
            if (accommodation.AutoApprove)
            {
                return Task.FromResult(new AccommodationAutoApproval
                {
                    AutoApproval = true
                });
            }
            else
            {
                return Task.FromResult(new AccommodationAutoApproval
                {
                    AutoApproval = false
                });
            }
        }

        public Model.Accommodation GetById(string id)
        {
            return _repository.GetById(id);
        }

        private async Task<bool> IsAvailable(DateTime startDate, DateTime endDate, string accommodationId)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            using var channel = GrpcChannel.ForAddress("https://reservation-service:443/",
                new GrpcChannelOptions { HttpHandler = handler });
            var client = new ReservationGRPCService.ReservationGRPCServiceClient(channel);
            try
            {
                var reply = await client.CheckIfAccommodationIsAvailableAsync(new AvailabilityPeriod
                {
                    AccommodationId = accommodationId,
                    StartDate = startDate.ToString(),
                    EndDate = endDate.ToString()
                });

                return reply.Available;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

        }

        public async Task<List<Model.Accommodation>> SearchAccomodation(SearchDTO searchDTO)
        {
            List<Model.Accommodation> allAccomodations = _repository.GetAll();
            List<Model.Accommodation> searchedAccomodations = new List<Model.Accommodation>();
            DateTime searchDTOCheckIn = DateTime.ParseExact(searchDTO.CheckIn, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime searchDTOCheckOut = DateTime.ParseExact(searchDTO.CheckOut, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            foreach (Model.Accommodation accommodation in allAccomodations)
            {
                if (accommodation.Address.City == searchDTO.Location)
                {
                    if (accommodation.MaxGuests >= searchDTO.NumberOfGuests && searchDTO.NumberOfGuests >= accommodation.MinGuests)
                    {
                        if (searchDTOCheckIn >= accommodation.StartSeasonDate && searchDTOCheckOut <= accommodation.EndSeasonDate)
                        {
                            bool available = await IsAvailable(searchDTOCheckIn, searchDTOCheckOut, accommodation.Id);
                            if (available)
                            {
                                accommodation.AccomodationPrice.FinalPrice = GetFinalAccomodationCost(accommodation, searchDTO);
                                searchedAccomodations.Add(accommodation);
                            }
                        }
                    }
                }
            }
            if (searchedAccomodations != null)
            {
                return searchedAccomodations;
            }
            else
            {
                return null;
            }
        }

        public double GetFinalAccomodationCost(Model.Accommodation accommodation, SearchDTO searchDto)
        {
            DateTime searchDTOCheckIn = DateTime.ParseExact(searchDto.CheckIn, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime searchDTOCheckOut = DateTime.ParseExact(searchDto.CheckOut, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            int numberOfDays = (searchDTOCheckOut - searchDTOCheckIn).Days;
            int weekends = 0;
            int holidays = 0;
            int summerDays = 0;
            double FinalPrice = accommodation.AccomodationPrice.FinalPrice;

            if (accommodation.AccomodationPrice.WeekendCost == false && accommodation.AccomodationPrice.HolidayCost == false && accommodation.AccomodationPrice.SummerCost == false)
            {
                if (accommodation.AccomodationPrice.PricePerAccomodation)
                {
                    FinalPrice = accommodation.AccomodationPrice.FinalPrice * (numberOfDays - 1);
                }
                else if (accommodation.AccomodationPrice.PricePerGuest)
                {
                    FinalPrice = (accommodation.AccomodationPrice.FinalPrice * searchDto.NumberOfGuests) * (numberOfDays - 1);
                }
            }

            if (accommodation.AccomodationPrice.WeekendCost)
            {

                for (int i = 0; i < numberOfDays; i++)
                {
                    if (searchDTOCheckIn.AddDays(i).DayOfWeek == DayOfWeek.Saturday)
                    { weekends++; }
                    if (searchDTOCheckOut.AddDays(i).DayOfWeek == DayOfWeek.Sunday)
                    { weekends++; }
                }

            }

            if (accommodation.AccomodationPrice.HolidayCost)
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
                        if (searchDTOCheckIn.AddDays(i) == holiday)
                        {
                            holidays++;
                        }
                    }
                }
            }

            if (accommodation.AccomodationPrice.SummerCost)
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
                        if (searchDTOCheckIn.AddDays(j) >= startSummer[i] && searchDTOCheckIn.AddDays(j) <= endSummer[i])
                        {
                            summerDays++;
                        }
                    }

                }
            }

            if (accommodation.AccomodationPrice.WeekendCost == true || accommodation.AccomodationPrice.HolidayCost == true || accommodation.AccomodationPrice.SummerCost || true)
            {
                if (accommodation.AccomodationPrice.PricePerAccomodation)
                {
                    FinalPrice = accommodation.AccomodationPrice.FinalPrice * (numberOfDays - 1) + (accommodation.AccomodationPrice.FinalPrice * 0.2) * (weekends + holidays + summerDays);

                }
                else if (accommodation.AccomodationPrice.PricePerGuest)
                {
                    FinalPrice = (accommodation.AccomodationPrice.FinalPrice * searchDto.NumberOfGuests) * (numberOfDays - 1) + (accommodation.AccomodationPrice.FinalPrice * 0.2 * searchDto.NumberOfGuests) * (weekends + holidays + summerDays);
                }
            }

            return FinalPrice;

        }

        public override Task<UserId> UpdateDistinguishedHostAccommodations(HostIdAndDistinguishedStatus hostIdAndDistinguishedStatus, ServerCallContext context)
        {
            using var scope = _tracer.BuildSpan("UpdateDistinguishedHostAccommodations").StartActive(true);
            List<Model.Accommodation> hostAccommodations = _repository.GetByHostId(hostIdAndDistinguishedStatus.Id);
            foreach (Model.Accommodation accommodation in hostAccommodations)
            {
                accommodation.IsDistinguishedHost = hostIdAndDistinguishedStatus.HostStatus;
                _repository.Update(accommodation);
            }

            return Task.FromResult(new UserId
            {
                Id = hostIdAndDistinguishedStatus.Id
            });
        }

        public List<Model.Accommodation> GetAll()
        {
            return _repository.GetAll();
        }
       
        public override Task<AccommodationGRPC> GetAccommodationGRPC(AccommodationId id, ServerCallContext context)
        {
            using var scope = _tracer.BuildSpan("GetAccommodationGRPC").StartActive(true);
            Model.Accommodation accommodation = _repository.GetById(id.Id);
            string address = accommodation.Address.Street + ',' + accommodation.Address.Number + ',' + accommodation.Address.City + ',' + accommodation.Address.Country;

            return Task.FromResult(new AccommodationGRPC
            {
                Name = accommodation.Name,
                StartSeason = accommodation.StartSeasonDate.ToString(),
                EndSeason = accommodation.EndSeasonDate.ToString(),
                Price = accommodation.AccomodationPrice.FinalPrice,
                PricePerGuest = accommodation.AccomodationPrice.PricePerGuest,
                PricePerAccomodation = accommodation.AccomodationPrice.PricePerAccomodation,
                HolidayCost = accommodation.AccomodationPrice.HolidayCost,
                WeekendCost = accommodation.AccomodationPrice.WeekendCost,
                SummerCost = accommodation.AccomodationPrice.SummerCost,
                Address = address
            });
                

        }


        public async Task<bool> Update(UpdateAccommodationDTO updateAccommodationDTO)
        {
            bool hasReservation = await AccommodatioHasReservation(updateAccommodationDTO.Id);
            if (hasReservation == false)
            {
                CultureInfo culture = CultureInfo.CreateSpecificCulture("sr-Cyrl-Rs");
                Model.Accommodation accommodation = GetById(updateAccommodationDTO.Id);
                accommodation.AccomodationPrice.FinalPrice = updateAccommodationDTO.Price;
                accommodation.StartSeasonDate = DateTime.ParseExact(updateAccommodationDTO.StartSeason, "yyyy-MM-dd", culture);
                accommodation.EndSeasonDate = DateTime.ParseExact(updateAccommodationDTO.EndSeason, "yyyy-MM-dd", culture);

                _repository.Update(accommodation);
                return true;
            }
            else         
            { 
                throw new Exception("You have reservation for that period"); 
            }

        }

        public async Task<bool> AccommodatioHasReservation(string id)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            using var channel = GrpcChannel.ForAddress("https://reservation-service:443",
                new GrpcChannelOptions { HttpHandler = handler });
            var client = new ReservationGRPCService.ReservationGRPCServiceClient(channel);
            var reply = await client.AccommodatioHasReservationAsync(new AccId
            {
                Id = id,

            });

            return reply.Reservation;
        }

        public override Task<AccommodationNames> GetAccommodationNames(AccommodationIds ids, ServerCallContext context)
        {
            List<string> names = new List<string>();
            foreach(string id in ids.Ids)
            {
                Accommodation.Model.Accommodation accomm = _repository.GetById(id);
                names.Add(accomm.Name);
            }

            AccommodationNames namesMessage = new AccommodationNames();
            namesMessage.Names.AddRange(names);

            return Task.FromResult(namesMessage);

        }

    }
}
