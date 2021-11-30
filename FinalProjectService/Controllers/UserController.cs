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
    public class UserController : ApiController
    {
        public async Task<IEnumerable<User>> Get()
        {
            return await Models.User.ReadAllAsync();
        }

        [Route("api/user/{username}")]
        public async Task<User> Get(string username)
        {
            var user = await Models.User.ReadAsync(username);
            if (user != null)
            {
                return user;
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        public async Task<User> Post([FromBody] UserRequest user)
        {
            try
            {
                return await Models.User.CreateAsync(user);
            }
            catch (UserAlreadyExistsException)
            {
                throw new HttpResponseException(HttpStatusCode.Conflict);
            }
        }

        [Route("api/user/{username}")]
        public async void Put(string username, [FromBody] UserRequest req)
        {
            await Models.User.UpdateAsync(username, req.Password);
        }

        [Route("api/user/{username}")]
        public async Task Delete(string username)
        {
            await Models.User.DeleteAsync(username);
        }

        [HttpPost]
        [Route("api/user/{username}/cart/product/{productId}")]
        public async Task AddProductToCartAsync(string username, string productId)
        {
            var user = Models.User.ReadAsync(username);
            var product = Product.ReadAsync(ObjectId.Parse(productId));

            var userFound = await user;
            var productFound = await product;

            if (userFound != null && productFound != null)
            {
                await Models.User.AddProductToCartAsync(userFound, productFound);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

        }

        [HttpDelete]
        [Route("api/user/{username}/cart/product/{productId}")]
        public async Task Delete(string username, string productId)
        {
            var user = Models.User.ReadAsync(username);
            var product = Product.ReadAsync(ObjectId.Parse(productId));

            var userFound = await user;
            var productFound = await product;

            if (userFound != null && productFound != null)
            {
                await Models.User.RemoveProductToCartAsync(userFound, productFound);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

        }
    }
}