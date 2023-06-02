using Accommodation.Adapters;
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

namespace Accommodation.Services
{
    public class AccommodationService : AccommodationGRPCService.AccommodationGRPCServiceBase
    {
        private readonly Repository.AccommodationRepository _repository;

        public AccommodationService(Repository.AccommodationRepository repository)
        {
            _repository = repository;
        }

        public void Create(DTO.CreateAccommodationDTO dto, StringValues hostId, List<IFormFile> photos)
        {
            Model.Accommodation accommodation = Adapters.CreateAccommodationAdapter.CreateAccommodaitonDtoToObject(dto, hostId);

            _repository.Create(accommodation, photos);
        }

        public async Task<List<byte[]>> GetAccommodationPhotos(string accommId)
        {
            return await _repository.GetAccommodationPhotos(accommId);
        }

        public override Task<Deleted>   DeleteAccwithoutHost(UserId userId, ServerCallContext context)
        {
           
            _repository.DeleteAccWithoutHost(userId.Id);
            return Task.FromResult(new Deleted
            {
                IsDeleted = true
            });
        }
        
        public Model.Accommodation GetById(string id)
        {
            return _repository.GetById(id);
        }

        public List<Model.Accommodation> SearchAccomodation(SearchDTO searchDTO)
        {
            List<Model.Accommodation> allAccomodations = _repository.GetAll();
            List<Model.Accommodation> searchedAccomodations = new List<Model.Accommodation>();
            foreach(Model.Accommodation accommodation in allAccomodations)
            {
                if(accommodation.Address == searchDTO.Location && accommodation.MaxGuests >= searchDTO.numberOfGuests && searchDTO.numberOfGuests >= accommodation.MinGuests)
                {
                    searchedAccomodations.Add(accommodation);
                }
            }
            if(searchedAccomodations != null)
            {
                return searchedAccomodations;
            }
            else
            {
                return null;
            }
            
        }

        public List<Model.Accommodation> GetAll()
        {
            return _repository.GetAll();
        }
       
        public override Task<AccommodationGRPC> GetAccommodationGRPC(AccommodationId id, ServerCallContext context)
        {
            Model.Accommodation accommodation = _repository.GetById(id.Id);

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
                SummerCost = accommodation.AccomodationPrice.SummerCost
            }) ;
                

        }

      
        public async void Update( UpdateAccommodationDTO updateAccommodationDTO)
        {
            bool hasReservation = await AccommodatioHasReservation(updateAccommodationDTO.Id);
            if (hasReservation == true) throw new Exception("You have reservation for that period");
            CultureInfo culture = CultureInfo.CreateSpecificCulture("sr-Cyrl-Rs");
            Model.Accommodation accommodation = GetById(updateAccommodationDTO.Id);
            accommodation.AccomodationPrice.FinalPrice = updateAccommodationDTO.Price;
            accommodation.StartSeasonDate = DateTime.ParseExact(updateAccommodationDTO.StartSeason, "yyyy-MM-dd", culture);
            accommodation.EndSeasonDate = DateTime.ParseExact (updateAccommodationDTO.EndSeason, "yyyy-MM-dd", culture);

            _repository.Update(accommodation);

        }

        public async Task<bool> AccommodatioHasReservation(string id)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            using var channel = GrpcChannel.ForAddress("https://localhost:5003",
                new GrpcChannelOptions { HttpHandler = handler });
            var client = new ReservationGRPCService.ReservationGRPCServiceClient(channel);
            var reply = await client.AccommodatioHasReservationAsync(new AccId
            {
                Id = id,

            });

            return reply.Reservation;
        }




    }
}
