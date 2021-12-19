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
        [HttpGet]
        [Route("api/product/user/{userId}")]
        public async Task<List<Product>> GetAllUserProducts(string userId)
        {
            var crud = new ProductHandler();

            var productFounds = await crud.ReadAllAsync(ObjectId.Parse(userId));
            if (productFounds != null)
            {
                return productFounds;
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }


        [Route("api/product/user/{userId}")]
        public async Task<string> Post(string userId, [FromBody] ProductRequest product)
        {
            var crud = new ProductHandler();
            product.userId = ObjectId.Parse(userId);

            return await crud.CreateAsync(new Product(product));    
        }

        [Route("api/product/{productId}")]
        public async Task Put(string productId, [FromBody] ProductRequest product)
        {
            var crud = new ProductHandler();
            var productIdParsed = ObjectId.Parse(productId);
            var productFound = await crud.ReadAsync<Product>("product", productIdParsed);

            if (productFound != null)
            {
                await crud.UpdateAsync(productFound.id, new Product(product));
                var productUpdated = await crud.ReadAsync<Product>("product", productIdParsed);
                if(productUpdated.stock == 0)
                {
                    await DeleteProductFromAllCarts(new UserHandler(), productUpdated);
                }
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
                await crud.DeleteAsync<Product>("product", productFound.id);

                await DeleteProductFromAllCarts(crud, productFound);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        private static async Task DeleteProductFromAllCarts(UserHandler crud, Product product)
        {
            var users = await crud.ReadAllAsync<User>("user");

            var tasks = users.Where(user => user.cart.Contains(product.id)).Select(user => crud.RemoveProductFromCartAsync(user, product));
            await Task.WhenAll(tasks);
        }
    }
}