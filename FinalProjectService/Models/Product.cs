using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FinalProjectService.Models
{
    public interface IProduct
    {
        string name { get; set; }
        decimal price { get; set; }
        string description { get; set; }
        int stock { get; set; }
        string category { get; set; }
        string seller { get; set; } 

        ObjectId userId { get; set; }
    }

    public class ProductRequest : IProduct
    {
        public string name { get; set; }
        public decimal price { get; set; }
        public string description { get; set; }
        public int stock { get; set; }
        public string seller { get; set; }
        public string category { get; set; }
        public ObjectId userId { get; set; }

    }

    public class Product : IProduct
    {
        [BsonId]
        public ObjectId id { get; set; } = ObjectId.GenerateNewId();

        public string name
        {
            get; set;
        }
        public decimal price
        {
            get; set;
        }
        public string description
        {
            get; set;
        }
        public int stock
        {
            get; set;
        }
        public string category { get; set; }

        public ObjectId userId { get; set; }

        public string image { get; set; } = "";
        public string seller { get; set; }
        public Product(IProduct product)
        {
            this.name = product.name;
            this.description = product.description;
            this.stock = product.stock;
            this.price = product.price;
            this.userId = product.userId;
            this.seller = product.seller;
            this.category = product.category;
        }
    }
}