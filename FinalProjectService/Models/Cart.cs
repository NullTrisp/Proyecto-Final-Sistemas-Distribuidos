using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProjectService.Models
{
    public interface ICart
    {
        IEnumerable<ObjectId> Products { get; set; }
    }
    public class Cart : ICart
    {
        public IEnumerable<ObjectId> Products { get; set; } = new List<ObjectId>();
    }
}