using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FinalProjectService.Classes
{
    public class DbHandler
    {
        public static IMongoCollection<T> GetCollection<T>(string host, string Db, string collection)
        {
            var client = new MongoClient($"mongodb://{host}");
            var db = client.GetDatabase(Db);
            return db.GetCollection<T>(collection);
        }
    }
}