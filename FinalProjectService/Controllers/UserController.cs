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
    public class UserController : ApiController
    {
        public IEnumerable<User> Get()
        {
            return Models.User.ReadAll();
        }

        [Route("api/user/{username}")]
        public User Get(string username)
        {
            return Models.User.Read(username);
        }

        public User Post([FromBody] UserRequest user)
        {
            var userCreated = Models.User.Create(user);

            if (userCreated != null)
            {
                return userCreated;
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.Conflict);
            }
        }

        [Route("api/user/{username}")]
        public void Put(string username, [FromBody] UserRequest req)
        {
            Models.User.Update(username, req.Password);
        }

        [Route("api/user/{username}")]
        public void Delete(string username)
        {
            Models.User.Delete(username);
        }
    }
}