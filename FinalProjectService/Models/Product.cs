using FinalProjectService.Classes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProjectService.Models
{
    public interface IProduct
    {
        string Name { get; set; }
        decimal Price { get; set; }
        string Description { get; set; }
        int Stock { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Product : RealmObject, IProduct
    {
        public static readonly string collection = "product";
        [PrimaryKey]
        [Indexed]
        public string Name
        {
            get; set;
        }
        public decimal Price
        {
            get; set;
        }
        [Required]
        public string Description
        {
            get; set;
        }
        public int Stock
        {
            get; set;
        }

        public static Product Create(IProduct product)
        {
            var mongoCollection = DbHandler.GetCollection<Product>("localhost:27017", "amazon", collection);

            mongoCollection.InsertOne(new Product()
            {
                Name = product.Name,
                Description = product.Description,
                Stock = product.Stock,
                Price = product.Price
            });

            return Read(product.Name);
        }

        public static Product Read(string productName)
        {
            var mongoCollection = DbHandler.GetCollection<Product>("localhost:27017", "amazon", collection);
            var filter = Builders<Product>.Filter.Eq("Name", productName);
            var documentFound = mongoCollection.Find(filter).FirstOrDefault();

            if (documentFound != null)
            {
                return new Product()
                {
                    Name = documentFound.Name,
                    Description = documentFound.Description,
                    Price = documentFound.Price,
                    Stock = documentFound.Stock
                };
            }
            else
            {
                return null;
            }
        }

        public static IEnumerable<Product> ReadAll()
        {
            var mongoCollection = DbHandler.GetCollection<Product>("localhost:27017", "amazon", collection);

            return mongoCollection.Find(_ => true).ToList().ToArray()
                .Select(el => new Product()
                {
                    Name = el.Name,
                    Description = el.Description,
                    Stock = el.Stock,
                    Price = el.Price
                });
        }

        public Product Update(IProduct product)
        {
            var mongoCollection = DbHandler.GetCollection<Product>("localhost:27017", "amazon", collection);
            var filter = Builders<Product>.Filter.Eq("Name", this.Name);
            var updateDefinition = Builders<Product>.Update.Set(rec => rec.Description, product.Description);
            var productFound = mongoCollection.FindOneAndUpdate<Product>(filter, updateDefinition, new FindOneAndUpdateOptions<Product>
            {
                ReturnDocument = ReturnDocument.After
            });

            if (productFound != null)
            {
                return productFound;
            }
            else
            {
                return null;
            }
        }

        public void Delete()
        {
            var mongoCollection = DbHandler.GetCollection<Product>("localhost:27017", "amazon", collection);
            var filter = Builders<Product>.Filter.Eq("Name", this.Name);
            mongoCollection.FindOneAndDelete<Product>(filter);
        }
    }
}