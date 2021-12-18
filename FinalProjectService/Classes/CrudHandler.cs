using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectService.Classes
{
    public class CrudHandler
    {
        internal IMongoDatabase db;

        public CrudHandler()
        {
            var client = new MongoClient(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString);
            this.db = client.GetDatabase("amazon");
        }

        public async Task CreateAsync<T>(string collectionName, T record)
        {
            var collection = this.db.GetCollection<T>(collectionName);
            await collection.InsertOneAsync(record);
        }

        public async Task<T> ReadAsync<T>(string collectionName, ObjectId id)
        {
            var collection = this.db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("id", id);
            return (await collection.FindAsync<T>(filter)).FirstOrDefault();
        }

        public async Task<IEnumerable<T>> ReadAllAsync<T>(string collectionName)
        {
            var collection = this.db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Empty;
            return (await collection.FindAsync<T>(filter)).ToList();
        }

        public async Task DeleteAsync<T>(string collectionName, ObjectId id)
        {
            var collection = this.db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("id", id);

            await collection.FindOneAndDeleteAsync<T>(filter);
        }
    }
}