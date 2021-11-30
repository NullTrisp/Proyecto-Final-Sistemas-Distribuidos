using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using FinalProjectService.Models;
using MongoDB.Bson;

namespace FinalProjectService.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProductController : ApiController
    {
        public async Task<IEnumerable<Product>> Get()
        {
            return await Product.ReadAllAsync();
        }

        [Route("api/product/{productId}")]
        public async Task<Product> Get(string productId)
        {
            var productFound = await Product.ReadAsync(ObjectId.Parse(productId));
            if (productFound != null)
            {
                return productFound;
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        public async Task<Product> Post([FromBody] Product product)
        {
            return await Product.CreateAsync(product);
        }

        [Route("api/product/{productId}")]
        public async Task<Product> Put(string productId, [FromBody] Product product)
        {
            var id = ObjectId.Parse(productId);
            var productFound = await Product.ReadAsync(id);

            if (productFound != null)
            {
                return await Product.UpdateAsync(id, product);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [Route("api/product/{productId}")]
        public async void Delete(string productId)
        {
            var id = ObjectId.Parse(productId);
            var productFound = await Product.ReadAsync(id);
            if (productFound != null)
            {
                await Product.DeleteAsync(id);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}