using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FinalProjectService.Models
{
    public interface IProduct
    {
        string Name { get; set; }
        decimal Price { get; set; }
        string Description { get; set; }
        int Stock { get; set; }

        ObjectId UserId { get; set; }
    }

    public class ProductRequest : IProduct
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public ObjectId UserId { get; set; }

    }

    public class Product : IProduct
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        public string Name
        {
            get; set;
        }
        public decimal Price
        {
            get; set;
        }
        public string Description
        {
            get; set;
        }
        public int Stock
        {
            get; set;
        }

        public ObjectId UserId { get; set; }

        public string Image { get; set; } = "";
        public Product(IProduct product)
        {
            this.Name = product.Name;
            this.Description = product.Description;
            this.Stock = product.Stock;
            this.Price = product.Price;
            this.UserId = product.UserId;
        }
    }
}