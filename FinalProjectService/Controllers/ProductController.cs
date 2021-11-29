using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FinalProjectService.Models;

namespace FinalProjectService.Controllers
{
    public class ProductController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<Product> Get()
        {
            return Product.ReadAll();
        }

        // GET api/<controller>/5
        [Route("api/product/{productName}")]
        public Product Get(string productName)
        {
            var productFound = Product.Read(productName);
            if (productName != null)
            {
                return productFound;
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        // POST api/<controller>
        public Product Post([FromBody] Product value)
        {
            if (Product.Read(value.Name) == null)
            {
                return Product.Create(value);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.Conflict);
            }
        }

        // PUT api/<controller>/5
        [Route("api/product/{productName}")]
        public Product Put(string productName, [FromBody] Product product)
        {
            var productFound = Product.Read(productName);

            if (productFound != null)
            {
                return productFound.Update(product);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        // DELETE api/<controller>/5
        [Route("api/product/{productName}")]
        public void Delete(HttpRequestMessage request, string productName)
        {
            var productFound = Product.Read(productName);
            if (productFound != null)
            {
                productFound.Delete();
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}