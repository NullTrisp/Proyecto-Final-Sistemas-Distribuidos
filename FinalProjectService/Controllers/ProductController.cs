using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FinalProjectService.Models;
using MongoDB.Bson;

namespace FinalProjectService.Controllers
{
    public class ProductController : ApiController
    {
        public IEnumerable<Product> Get()
        {
            return Product.ReadAll();
        }

        [Route("api/product/{productId}")]
        public Product Get(string productId)
        {
            var productFound = Product.Read(ObjectId.Parse(productId));
            if (productFound != null)
            {
                return productFound;
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        public Product Post([FromBody] Product product)
        {
            return Product.Create(product);
        }

        [Route("api/product/{productId}")]
        public Product Put(string productId, [FromBody] Product product)
        {
            var id = ObjectId.Parse(productId);
            var productFound = Product.Read(id);

            if (productFound != null)
            {
                return Product.Update(id, product);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [Route("api/product/{productId}")]
        public void Delete(string productId)
        {
            var id = ObjectId.Parse(productId);
            var productFound = Product.Read(id);
            if (productFound != null)
            {
                Product.Delete(id);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}