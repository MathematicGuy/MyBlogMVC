using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Web.Repository;
using System.Net;

namespace MyBlog.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        // allow the ImageRepository to be used in the ImagesController
        private readonly IImageRepository imageRepository;

        // constructor for the ImageController
        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            // call imageRepository to upload the image
            var imageURL = await imageRepository.UploadAsync(file);
            
            // return error msg if null and reutrn the image URL if true
            if (imageURL == null)
            {
                return Problem("Something went wrong", null, (int) HttpStatusCode.InternalServerError);
            }

            return new JsonResult(new { link = imageURL});
        }
        // use a Post command to post it to a Image Hosting Service
    }
}
