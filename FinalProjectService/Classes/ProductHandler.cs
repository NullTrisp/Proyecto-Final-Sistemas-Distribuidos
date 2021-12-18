using FinalProjectService.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProjectService.Classes
{
    public class ProductHandler : CrudHandler
    {
        public ProductHandler() : base()
        {

        }
        public async Task UpdateAsync(ObjectId id, Product record)
        {
            var collection = db.GetCollection<Product>("product");

            FilterDefinition<Product> filter(Product product) => Builders<Product>.Filter.Eq("id", product.id);
            var updateDefinition = Builders<Product>.Update.Set(rec => rec.description, record.description)
                .Set(rec => rec.name, record.name)
                .Set(rec => rec.stock, record.stock)
                .Set(rec => rec.price, record.price)
                .Set(rec => rec.category, record.category);

            await collection.FindOneAndUpdateAsync(filter(await ReadAsync<Product>("product", id)), updateDefinition);
        }

        public async Task<string> CreateAsync(Product record)
        {
            var collection = this.db.GetCollection<Product>("product");
            await collection.InsertOneAsync(record);
            var userFilter = Builders<Product>.Filter.Eq("userId", record.userId);
            var nameFilter = Builders<Product>.Filter.Eq("name", record.name);
            var value = (await collection.FindAsync<Product>(nameFilter & userFilter)).FirstOrDefault();
            return value.id.ToString();
        }

        public async Task<List<Product>> ReadAllAsync(ObjectId userId)
        {
            var collection = this.db.GetCollection<Product>("product");
            var userFilter = Builders<Product>.Filter.Eq("userId", userId);
            return (await collection.FindAsync<Product>(userFilter)).ToList();
        }
    }
}