using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using FinalProjectService.Classes;
using FinalProjectService.Models;
using MongoDB.Bson;

namespace FinalProjectService.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProductController : ApiController
    {
        public async Task<IEnumerable<Product>> Get()
        {
            var crud = new CrudHandler();

            return await crud.ReadAllAsync<Product>("product");
        }

        [Route("api/product/{productId}")]
        public async Task<Product> Get(string productId)
        {
            var crud = new CrudHandler();

            var productFound = await crud.ReadAsync<Product>("product", ObjectId.Parse(productId));
            if (productFound != null)
            {
                return productFound;
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }


        [Route("api/product/user/{userId}")]
        public async Task Post(string userId, [FromBody] ProductRequest product)
        {
            var crud = new CrudHandler();
            product.UserId = ObjectId.Parse(userId);

            await crud.CreateAsync("product", new Product(product));
        }

        [Route("api/product/{productId}")]
        public async Task Put(string productId, [FromBody] ProductRequest product)
        {
            var crud = new ProductHandler();
            var productFound = await crud.ReadAsync<Product>("product", ObjectId.Parse(productId));

            if (productFound != null)
            {
                await crud.UpdateAsync(productFound.Id, new Product(product));
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [Route("api/product/{productId}")]
        public async Task Delete(string productId)
        {
            var crud = new UserHandler();

            var productFound = await crud.ReadAsync<Product>("product", ObjectId.Parse(productId));
            if (productFound != null)
            {
                await crud.DeleteAsync<Product>("product", productFound.Id);
                var users = await crud.ReadAllAsync<User>("user");

                var tasks = users.Where(user => user.Cart.Contains(productFound.Id)).Select(user => crud.RemoveProductToCartAsync(user, productFound));
                await Task.WhenAll(tasks);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}