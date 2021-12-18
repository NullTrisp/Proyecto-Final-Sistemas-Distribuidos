using System;
using System.Linq;
using System.Threading.Tasks;
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

            var filter = Builders<User>.Filter.Eq("username", record.username);
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

        public async Task<User> Authentificate(UserCredentials credentials)
        {
            var authUser = new User(credentials);
            var collection = db.GetCollection<User>("user");
            var filterUsername = Builders<User>.Filter.Eq("username", authUser.username);
            var filterPassword = Builders<User>.Filter.Eq("password", authUser.password);

            var user = (await collection.FindAsync<User>(filterUsername & filterPassword)).FirstOrDefault();

            if (user != null) return user;
            else throw new Exception("Username or password incorrect");
        }

        //private Task<User> GetAllUserData()

        public async Task UpdateAsync(ObjectId id, User record)
        {
            var collection = db.GetCollection<User>("user");

            FilterDefinition<User> filter(User user) => Builders<User>.Filter.Eq("id", user.id);
            var updateDefinition = Builders<User>.Update.Set(rec => rec.password, record.password);

            await collection.FindOneAndUpdateAsync(filter(await ReadAsync<User>("user", id)), updateDefinition);
        }

        public async Task<User> AddProductToCartAsync(User user, Product product)
        {
            var collection = db.GetCollection<User>("user");

            if (!user.cart.Contains(product.id))
            {
                FilterDefinition<User> filter(User userToFind) => Builders<User>.Filter.Eq("id", userToFind.id);
                var updateDefinition = Builders<User>.Update.Push("cart", product.id);

                await collection.FindOneAndUpdateAsync(filter(user), updateDefinition);
                return (await collection.FindAsync<User>(filter(user))).FirstOrDefault();
            } else
            {
                return user;
            }
        }

        public async Task<User> RemoveProductFromCartAsync(User user, Product product)
        {
            var collection = db.GetCollection<User>("user");

            FilterDefinition<User> filter(User userToFind) => Builders<User>.Filter.Eq("id", userToFind.id);
            var updateDefinition = Builders<User>.Update.Pull("cart", product.id);
            await collection.FindOneAndUpdateAsync(filter(user), updateDefinition);
            return (await collection.FindAsync<User>(filter(user))).FirstOrDefault();
        }
    }
}