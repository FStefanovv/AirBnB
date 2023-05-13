using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accommodation.Model;
using MongoDB.Bson;
using System.IO;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver.GridFS;

namespace Accommodation.Repository
{
    public class AccommodationRepository
    {
        private readonly IDbContext _context;
        private IMongoCollection<Model.Accommodation> _accommodation;

        public AccommodationRepository(IDbContext context)
        {
            _context = context;
            _accommodation = _context.GetCollection<Model.Accommodation>("accommodation");
        }

        
        public void Create(Model.Accommodation accommodation, List<IFormFile> photos)
        {
            _accommodation.InsertOne(accommodation);
            foreach (IFormFile photo in photos)
            {
                _context.UploadImage(photo, accommodation.Id);
            }
        }

        public async Task<List<IFormFile>> GetAccommodationPhotos(string accommId)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq(x => x.Metadata["AccommObjectId"], accommId);
            return await _context.GetAccommodationPhotos(accommId);
        }

        public void DeleteAccWithoutHost(string id)
        {
            var filter = Builders<Model.Accommodation>.Filter.Eq("HostId", id);
            _accommodation.DeleteMany(filter); 

        }


        public List<Model.Accommodation> GetByHostId(string hostId)
        {
            return _accommodation.Find(accomodation => accomodation.HostId == hostId).ToList();
        }

        public Model.Accommodation GetById(string id)
        {
            return (Model.Accommodation)_accommodation.Find(user => user.Id == id);
        }
    }
}
