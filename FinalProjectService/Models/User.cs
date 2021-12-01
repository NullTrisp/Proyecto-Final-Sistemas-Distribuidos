using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

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

    public class User : IUser
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        public string Username { get; set; }

        public string Password { get; set; }

        public List<ObjectId> Cart
        {
            get; set;
        } = new List<ObjectId>();

        public User(IUser user)
        {
            this.Username = user.Username;
            this.Password = user.Password;
        }
    }
}