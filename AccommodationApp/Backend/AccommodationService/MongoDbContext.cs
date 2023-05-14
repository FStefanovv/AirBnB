using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace Accommodation
{
    public class MongoDbContext : IDbContext
    {
        private IMongoDatabase _db { get; set; }
        private MongoClient _mongoClient { get; set; }
        private GridFSBucket _gridFSBucket { get; set; }
        public MongoDbContext(IConfiguration configuration)
        {
            _mongoClient = new MongoClient(configuration.GetValue<string>("XWSDatabase:ConnectionString"));
            _db = _mongoClient.GetDatabase(configuration.GetValue<string>("XWSDatabase:DatabaseName"));
            _gridFSBucket = new GridFSBucket(_db);
        }

        public ObjectId UploadImage(IFormFile file, string accommObjectId)
        {
            using (var s = file.OpenReadStream())
            {
                var t = Task.Run<ObjectId>(() =>
                {
                    return _gridFSBucket.UploadFromStreamAsync(file.FileName, s,
                        new GridFSUploadOptions
                        {
                            Metadata = new BsonDocument { { "Type", "photo" }, { "AccommObjectId", accommObjectId } }
                        });
                });

                return t.Result;
            }
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }

        public async Task<List<byte[]>> GetAccommodationPhotos(string accommId)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq(x => x.Metadata["AccommObjectId"], accommId);

            var photos = new List<byte[]>();

            using (var cursor = await _gridFSBucket.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;

                    foreach (var fileInfo in batch)
                    {
                        var fileId = fileInfo.Id;


                        byte[] currentPhoto = await _gridFSBucket.DownloadAsBytesAsync(fileId);


                        photos.Add(currentPhoto);
                    }
                }

            }


            return photos;
        }
    }

    public interface IDbContext
    {
        Task<List<byte[]>> GetAccommodationPhotos(string accommId);
        IMongoCollection<T> GetCollection<T>(string name);
        ObjectId UploadImage(IFormFile file, string accommObjectId);
    }
}
