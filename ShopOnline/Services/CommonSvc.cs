using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnline.Services
{
    public class CommonSvc
    {
        public string UploadImageToCloudinary(IFormFile file)
        {
            Account account = new Account(
                "dp50hyprx",
                "919543544232649",
                "UCT8SrEd9xOE3FuTYo1f4AUamhk"
                );
            Cloudinary cloudinary = new Cloudinary(account);
            cloudinary.Api.Secure = true;

            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                bytes = stream.ToArray();
            }
            string base64 = Convert.ToBase64String(bytes);

            var prefix = @"data:image/png;base64,";
            var imagePath = prefix + base64;
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imagePath),
                Folder = "FlowerShop/img"
            };
            var uploadResult = cloudinary.Upload(uploadParams);

            return uploadResult.SecureUrl.AbsoluteUri;
        }
    }
}
