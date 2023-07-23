using Goba_Store.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goba_Store.Utility
{
    public static class ImageHelper
    {
        public static string UploadImage(IWebHostEnvironment environment, IFormFileCollection files)
        {
            var rootPath = environment.WebRootPath;
            var uploadedFile = rootPath + Constants.ImagePath;
            var fileName = Guid.NewGuid().ToString();
            var extention = Path.GetExtension(files[0].FileName);
            //create new one and save it
            using (FileStream stream = new FileStream(Path.Combine(uploadedFile, fileName + extention), FileMode.Create))
            {
                files[0].CopyTo(stream);
            }

            return fileName + extention;
        }


        public  static void DeleteImage(IWebHostEnvironment environment , Product product)
        {
            var rootPath = environment.WebRootPath;
            var uploadedFile = rootPath + Constants.ImagePath;
            var extention = Path.GetExtension(product.Image);
            //delete old image
            var oldImage = Path.Combine(uploadedFile, product.Image + extention);
            if (File.Exists(oldImage))
            {
                File.Delete(oldImage);
            }
        }
    }
}
