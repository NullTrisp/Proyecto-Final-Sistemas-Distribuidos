using FinalProjectService.Classes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Realms;
using Realms.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

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

    [BsonIgnoreExtraElements]
    public class User : RealmObject, IUser
    {
        private static IMongoCollection<User> collection = DbHandler.GetCollection<User>("user");
        [PrimaryKey]
        [Indexed]
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public User(IUser user)
        {
            this.Username = user.Username;
            this.Password = user.Password;
        }

        public User() { }

        public static User Create(IUser user)
        {
            if (Read(user.Username) != null)
            {
                return null;
            }
            else
            {
                var userToCreate = new User(user);
                collection.InsertOne(userToCreate);

                return Read(userToCreate.Username);
            }
        }

        public static User Read(string username)
        {
            var filter = Builders<User>.Filter.Eq("Username", username);
            var documentFound = collection.Find(filter).FirstOrDefault<User>();

            return documentFound;
        }

        public static IEnumerable<User> ReadAll()
        {
            return collection.Find(new BsonDocument()).ToList();
        }

        public static User Update(string username, string password)
        {
            var userFound = Read(username);

            if (userFound != null)
            {
                var filter = Builders<User>.Filter.Eq("Username", username);
                var updateDefinition = Builders<User>.Update.Set(rec => rec.Password, password);

                return collection.FindOneAndUpdate<User>(filter, updateDefinition, new FindOneAndUpdateOptions<User>
                {
                    ReturnDocument = ReturnDocument.After
                });
            }
            else
            {
                return null;
            }
        }

        public static void Delete(string username)
        {
            var filter = Builders<User>.Filter.Eq("Username", username);
            collection.FindOneAndDelete<User>(filter);
        }
    }
}