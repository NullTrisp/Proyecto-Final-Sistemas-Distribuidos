using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace FinalProjectService.Models
{
    public interface IUser
    {
        string username
        {
            get; set;
        }

        string password
        {
            get; set;
        }

        string email
        {
            get; set;
        }
    }

    public class UserCredentials
    {
        public string username
        {
            get; set;
        }

        public string password
        {
            get; set;
        }
    }

    public class UserCreationRequest : IUser
    {
        public string username { get; set; }

        public string password { get; set; }

        public string email { get; set; }
    }

    public class User : IUser
    {
        [BsonId]
        public ObjectId id { get; set; } = ObjectId.GenerateNewId();

        public string username { get; set; }

        public string password { get; set; }

        public string email { get; set; }

        public List<ObjectId> cart
        {
            get; set;
        } = new List<ObjectId>();

        public List<ObjectId> products
        {
            get; set;
        } = new List<ObjectId>();

        public User(IUser user)
        {
            this.username = user.username;
            this.password = user.password;
            this.email = user.email;
        }
        public User(UserCredentials credentials)
        {
            this.username = credentials.username;
            this.password = credentials.password;
            this.email = "";
        }
    }
}