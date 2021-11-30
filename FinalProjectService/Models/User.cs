using FinalProjectService.Classes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectService.Models
{
    public interface IUser
    {
        string Username
        {
            get; set;
        }

        string Password
        {
            get; set;
        }
    }

    public class UserRequest : IUser
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }

    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException() : base("User already exists")
        {
        }
    }

    [BsonIgnoreExtraElements]
    public class User : RealmObject, IUser
    {
        private static readonly IMongoCollection<User> collection = DbHandler.GetCollection<User>("user");
        private static readonly Func<string, FilterDefinition<User>> filterByUsername = (string username) => Builders<User>.Filter.Eq("Username", username);

        [PrimaryKey]
        [Indexed]
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public IList<string> Cart
        {
            get;
        }

        public User(IUser user)
        {
            this.Username = user.Username;
            this.Password = user.Password;
        }

        public User() { }

        public static async Task<User> CreateAsync(IUser user)
        {
            if (await ReadAsync(user.Username) != null)
            {
                throw new UserAlreadyExistsException();
            }
            else
            {
                var userToCreate = new User(user);
                await collection.InsertOneAsync(userToCreate);

                return await ReadAsync(userToCreate.Username);
            }
        }

        public static async Task<User> ReadAsync(string username)
        {
            return (await collection.FindAsync(filterByUsername(username))).FirstOrDefault();
        }

        public static async Task<IEnumerable<User>> ReadAllAsync()
        {
            return (await collection.FindAsync(new BsonDocument())).ToList();
        }

        public static async Task<User> UpdateAsync(string username, string password)
        {
            if (await ReadAsync(username) != null)
            {
                var updateDefinition = Builders<User>.Update.Set(rec => rec.Password, password);

                return await collection.FindOneAndUpdateAsync(filterByUsername(username), updateDefinition, new FindOneAndUpdateOptions<User>
                {
                    ReturnDocument = ReturnDocument.After
                });
            }
            else
            {
                return null;
            }
        }

        public static async Task DeleteAsync(string username)
        {
            await collection.FindOneAndDeleteAsync<User>(filterByUsername(username));
        }

        public static async Task AddProductToCartAsync(User user, Product product)
        {
            var updateDefinition = Builders<User>.Update.Push("Cart", product.Id);
            await collection.FindOneAndUpdateAsync(filterByUsername(user.Username), updateDefinition);
        }

        public static async Task RemoveProductToCartAsync(User user, Product product)
        {
            var updateDefinition = Builders<User>.Update.Pull("Cart", product.Id);
            await collection.FindOneAndUpdateAsync(filterByUsername(user.Username), updateDefinition);
        }
    }
}