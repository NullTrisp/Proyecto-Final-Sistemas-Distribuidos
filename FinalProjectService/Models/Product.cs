using FinalProjectService.Classes;
using MongoDB.Bson;
using MongoDB.Driver;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        private static readonly IMongoCollection<Product> collection = DbHandler.GetCollection<Product>("product");
        private static readonly Func<ObjectId, FilterDefinition<Product>> filterById = (ObjectId id) => Builders<Product>.Filter.Eq("Id", id);

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

        public static async Task<Product> CreateAsync(IProduct product)
        {
            var productToCreate = new Product(product);
            await collection.InsertOneAsync(productToCreate);

            return await ReadAsync(productToCreate.Id);
        }

        public static async Task<Product> ReadAsync(ObjectId id)
        {
            var documentFound = (await collection.FindAsync(filterById(id))).FirstOrDefault();

            if (documentFound != null)
            {
                return documentFound;
            }
            else
            {
                return null;
            }
        }

        public static async Task<IEnumerable<Product>> ReadAllAsync()
        {
            return (await collection.FindAsync(_ => true)).ToList();
        }

        public static async Task<Product> UpdateAsync(ObjectId id, IProduct product)
        {
            var updateDefinition = Builders<Product>.Update.Set(rec => rec.Description, product.Description)
                .Set(rec => rec.Name, product.Name)
                .Set(rec => rec.Stock, product.Stock)
                .Set(rec => rec.Price, product.Price);
            var productFound = await collection.FindOneAndUpdateAsync(filterById(id), updateDefinition, new FindOneAndUpdateOptions<Product>
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

        public static async Task DeleteAsync(ObjectId id)
        {
            await collection.FindOneAndDeleteAsync<Product>(filterById(id));
        }
    }
}