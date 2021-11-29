﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FinalProjectService.Classes
{
    public class DbHandler
    {
        public static IMongoCollection<T> GetCollection<T>(string collection)
        {
            var client = new MongoClient($"mongodb://localhost:27017");
            var db = client.GetDatabase("amazon");
            return db.GetCollection<T>(collection);
        }
    }
}