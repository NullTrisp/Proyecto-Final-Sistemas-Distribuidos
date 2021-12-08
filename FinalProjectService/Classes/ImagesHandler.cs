using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FinalProjectService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FinalProjectService.Classes
{
    public class ImagesHandler
    {
        private static readonly Account account = new Account("dv99vyr8e", "311814242524694", "fiw2I1hsxW9tweNGI32LBM9meN8");
        private static readonly Cloudinary cloudinary = new Cloudinary(account);

        public static async Task<ImageUploadResult> UploadProductImageAsync(string fileName, Stream file, Product product)
        {
            cloudinary.Api.Secure = true;
            return await cloudinary.UploadAsync(new ImageUploadParams()
            {
                File = new FileDescription(fileName, file),
                PublicId = product.Id.ToString()
            });
        }

        public static async Task DeleteProductImageAsync(Product product)
        {
            cloudinary.Api.Secure = true;
            await cloudinary.DeleteResourcesAsync(new string[] { product.Id.ToString() });
        }

    }
}