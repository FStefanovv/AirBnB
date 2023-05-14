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

        public void DeleteAccwithoutHost(string id)
        {
           
            _repository.DeleteAccWithoutHost(id);
        }
        
        public Model.Accommodation GetById(string id)
        {
            return _repository.GetById(id);
        }

        public override Task<AccommodationGRPC> GetAccommodationGRPC(AccommodationId id, ServerCallContext context) {
            AccommodationGRPC accommodation = _repository.GetByIdGRPC(id);

            return Task.FromResult(accommodation);
        } 

    }
}
