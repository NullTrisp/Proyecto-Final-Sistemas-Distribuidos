using System.Collections.Generic;
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
    public class UserController : ApiController
    {
        public async Task<IEnumerable<User>> Get()
        {
            var crud = new CrudHandler();
            return await crud.ReadAllAsync<User>("user");
        }

        [Route("api/user/{id}")]
        public async Task<User> Get(string id)
        {
            var crud = new UserHandler();
            var user = await crud.ReadAsync<User>("user", ObjectId.Parse(id));
            if (user != null)
            {
                return user;
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        public async Task Post([FromBody] UserCreationRequest user)
        {
            var crud = new UserHandler();

            try
            {
                await crud.CreateAsync(new User(user));
            }
            catch (UserAlreadyExistsException)
            {
                throw new HttpResponseException(HttpStatusCode.Conflict);
            }
        }

        [HttpPost]
        [Route("api/user/login")]
        public async Task<User> Login([FromBody] UserCredentials user)
        {
            var crud = new UserHandler();

            try
            {
                return await crud.Authentificate(user);
            }
            catch (UserAlreadyExistsException)
            {
                throw new HttpResponseException(HttpStatusCode.Conflict);
            }
        }

        [Route("api/user/{id}")]
        public async Task Put(string id, [FromBody] UserCredentials req)
        {
            var crud = new UserHandler();
            await crud.UpdateAsync(ObjectId.Parse(id), new User(req));
        }

        [Route("api/user/{id}")]
        public async Task Delete(string id)
        {
            var crud = new UserHandler();
            await crud.DeleteAsync<User>("user", ObjectId.Parse(id));
        }

        [HttpPost]
        [Route("api/user/{userId}/cart/product/{productId}")]
        public async Task<User> AddProductToCartAsync(string userId, string productId)
        {
            var crud = new UserHandler();

            var user = crud.ReadAsync<User>("user", ObjectId.Parse(userId));
            var product = crud.ReadAsync<Product>("product", ObjectId.Parse(productId));

            var userFound = await user;
            var productFound = await product;

            if (userFound != null && productFound != null)
            {
                return await crud.AddProductToCartAsync(userFound, productFound);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

        }

        [HttpDelete]
        [Route("api/user/{userId}/cart/product/{productId}")]
        public async Task<User> DeleteProductFromCartAsync(string userId, string productId)
        {
            var crud = new UserHandler();

            var user = crud.ReadAsync<User>("user", ObjectId.Parse(userId));
            var product = crud.ReadAsync<Product>("product", ObjectId.Parse(productId));

            var userFound = await user;
            var productFound = await product;

            if (userFound != null && productFound != null)
            {
                return await crud.RemoveProductFromCartAsync(userFound, productFound);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpDelete]
        [Route("api/user/{userId}/cart/product/")]
        public async Task<User> DeleteAllProductsFromCartAsync(string userId)
        {
            var crud = new UserHandler();

            var user = crud.ReadAsync<User>("user", ObjectId.Parse(userId));

            var userFound = await user;

            if (userFound != null)
            {
                return await crud.PurchaseAllItemsInCart(userFound);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}