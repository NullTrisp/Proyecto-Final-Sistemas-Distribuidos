﻿using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FinalProjectService.Classes
{
    public class CrudHandler
    {
        internal IMongoDatabase db;

        public CrudHandler()
        {
            var client = new MongoClient($"mongodb://localhost:27017");
            this.db = client.GetDatabase("amazon");
        }

        public async Task CreateAsync<T>(string collectionName, T record)
        {
            var collection = this.db.GetCollection<T>(collectionName);
            await collection.InsertOneAsync(record);
        }

        public async Task<T> ReadAsync<T>(string collectionName, ObjectId id)
        {
            var collection = this.db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("Id", id);
            return (await collection.FindAsync<T>(filter)).FirstOrDefault();
        }

        public async Task<IEnumerable<T>> ReadAllAsync<T>(string collectionName)
        {
            var collection = this.db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Empty;
            return (await collection.FindAsync<T>(filter)).ToList();
        }

        public async Task DeleteAsync<T>(string collectionName, ObjectId id)
        {
            var collection = this.db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("Id", id);

            await collection.FindOneAndDeleteAsync<T>(filter);
        }
    }
}