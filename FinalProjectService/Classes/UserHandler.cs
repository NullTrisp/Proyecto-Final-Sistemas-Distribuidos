using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FinalProjectService.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FinalProjectService.Classes
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException() : base("User already exists!") { }
    }
    public class UserHandler : CrudHandler
    {
        public UserHandler() : base()
        {

        }

        public async Task CreateAsync(User record)
        {
            var collection = db.GetCollection<User>("user");

            var filter = Builders<User>.Filter.Eq("Username", record.Username);
            var user = (await collection.FindAsync<User>(filter)).FirstOrDefault();
            if (user == null)
            {
                await collection.InsertOneAsync(record);
            }
            else
            {
                throw new UserAlreadyExistsException();
            }
        }

        public async Task UpdateAsync(ObjectId id, User record)
        {
            var collection = db.GetCollection<User>("user");

            FilterDefinition<User> filter(User user) => Builders<User>.Filter.Eq("Id", user.Id);
            var updateDefinition = Builders<User>.Update.Set(rec => rec.Password, record.Password);

            await collection.FindOneAndUpdateAsync(filter(await ReadAsync<User>("user", id)), updateDefinition);
        }

        public async Task AddProductToCartAsync(User user, Product product)
        {
            var collection = db.GetCollection<User>("user");

            if (!user.Cart.Contains(product.Id))
            {
                FilterDefinition<User> filter(User userToFind) => Builders<User>.Filter.Eq("Id", userToFind.Id);
                var updateDefinition = Builders<User>.Update.Push("Cart", product.Id);

                await collection.FindOneAndUpdateAsync(filter(user), updateDefinition);
            }
        }

        public async Task RemoveProductToCartAsync(User user, Product product)
        {
            var collection = db.GetCollection<User>("user");

            FilterDefinition<User> filter(User userToFind) => Builders<User>.Filter.Eq("Id", userToFind.Id);
            var updateDefinition = Builders<User>.Update.Pull("Cart", product.Id);
            await collection.FindOneAndUpdateAsync(filter(user), updateDefinition);
        }
    }
}