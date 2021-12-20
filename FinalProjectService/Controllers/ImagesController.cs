using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FinalProjectService.Classes;
using FinalProjectService.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FinalProjectService.Controllers
{
    public class ImagesController : ApiController
    {
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        // POST api/<controller>
        [Route("api/images/{productId}")]
        public async Task PostAsync(string productId)
        {
            var productHandler = new ProductHandler();
            ObjectId productObjectId = ObjectId.Parse(productId);
            var productFoundTask = productHandler.ReadAsync<Product>("product", productObjectId);

            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var request = HttpContext.Current.Request;
            if (request.Files.Count > 0)
            {
                var productFound = await productFoundTask;

                if (productFound != null)
                {
                    if (!productFound.image.Equals(""))
                    {
                        await ImagesHandler.DeleteProductImageAsync(productFound);
                    }
                    var postedFile = request.Files[0];
                    var imageUploaded = await ImagesHandler.UploadProductImageAsync(postedFile.FileName, postedFile.InputStream, productFound);

                    productFound.image = imageUploaded.SecureUrl.ToString();
                    await productHandler.UpdateAsync(productObjectId, productFound);
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [Route("api/images/{productId}")]
        public async Task DeleteAsync(string productId)
        {
            var productHandler = new ProductHandler();
            ObjectId productObjectId = ObjectId.Parse(productId);
            var productFound = await productHandler.ReadAsync<Product>("product", productObjectId);

            if (productFound != null)
            {
                productFound.image = "";
                await productHandler.UpdateAsync(productObjectId, productFound);
                await ImagesHandler.DeleteProductImageAsync(productFound);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}