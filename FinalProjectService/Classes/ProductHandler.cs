using FinalProjectService.Models;
using MongoDB.Bson;
using MongoDB.Driver;
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

            FilterDefinition<Product> filter(Product product) => Builders<Product>.Filter.Eq("Id", product.Id);
            var updateDefinition = Builders<Product>.Update.Set(rec => rec.Description, record.Description)
                .Set(rec => rec.Name, record.Name)
                .Set(rec => rec.Stock, record.Stock)
                .Set(rec => rec.Price, record.Price)
                .Set(rec => rec.Image, record.Image);

            await collection.FindOneAndUpdateAsync(filter(await ReadAsync<Product>("product", id)), updateDefinition);
        }

    }
}