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

        public async Task Post([FromBody] UserRequest user)
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

        [Route("api/user/{id}")]
        public async Task Put(string id, [FromBody] UserRequest req)
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
        public async Task AddProductToCartAsync(string userId, string productId)
        {
            var crud = new UserHandler();

            var user = crud.ReadAsync<User>("user", ObjectId.Parse(userId));
            var product = crud.ReadAsync<Product>("product", ObjectId.Parse(productId));

            var userFound = await user;
            var productFound = await product;

            if (userFound != null && productFound != null)
            {
                await crud.AddProductToCartAsync(userFound, productFound);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

        }

        [HttpDelete]
        [Route("api/user/{userId}/cart/product/{productId}")]
        public async Task Delete(string userId, string productId)
        {
            var crud = new UserHandler();

            var user = crud.ReadAsync<User>("user", ObjectId.Parse(userId));
            var product = crud.ReadAsync<Product>("product", ObjectId.Parse(productId));

            var userFound = await user;
            var productFound = await product;

            if (userFound != null && productFound != null)
            {
                await crud.RemoveProductToCartAsync(userFound, productFound);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}