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

    public class ProductRequest : IProduct
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
    }

    public class Product : RealmObject, IProduct
    {
        private static IMongoCollection<Product> collection = DbHandler.GetCollection<Product>("product");

        [PrimaryKey]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        [Indexed]
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

        public Product(IProduct product)
        {
            this.Name = product.Name;
            this.Description = product.Description;
            this.Stock = product.Stock;
            this.Price = product.Price;
        }

        public Product() { }

        public static Product Create(IProduct product)
        {
            var productToCreate = new Product(product);
            collection.InsertOne(productToCreate);

            return Read(productToCreate.Id);
        }

        public static Product Read(ObjectId id)
        {
            var filter = Builders<Product>.Filter.Eq("Id", id);
            var documentFound = collection.Find(filter).FirstOrDefault();

            if (documentFound != null)
            {
                return documentFound;
            }
            else
            {
                return null;
            }
        }

        public static IEnumerable<Product> ReadAll()
        {
            return collection.Find(_ => true).ToList().ToArray();
        }

        public static Product Update(ObjectId id, IProduct product)
        {
            var filter = Builders<Product>.Filter.Eq("Id", id);
            var updateDefinition = Builders<Product>.Update.Set(rec => rec.Description, product.Description);
            var productFound = collection.FindOneAndUpdate(filter, updateDefinition, new FindOneAndUpdateOptions<Product>
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

        public static void Delete(ObjectId id)
        {
            var filter = Builders<Product>.Filter.Eq("Id", id);
            collection.FindOneAndDelete<Product>(filter);
        }
    }
}