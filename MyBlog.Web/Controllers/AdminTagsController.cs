using Microsoft.AspNetCore.Mvc;
using MyBlog.Web.Data;
using MyBlog.Web.Models.Domain;
using MyBlog.Web.Models.ViewModels;

namespace MyBlog.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        // read from bloggieDbContext
        private readonly BloggieDbContext bloggieDbContext;

        // explain: constructor injection (dependency injection) - inject BloggieDbContext into AdminTagsController
        public AdminTagsController(BloggieDbContext bloggieDbContext)
        {
            // assign bloggieDbContext to this.bloggieDbContext
            this.bloggieDbContext = bloggieDbContext;
        }

        // GET: /AdminTags/Add
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // CREATE functionality
        //  POST is used to send data to a server to create/update a resource.
        [HttpPost]
        [ActionName("Add")] // if they're not the same Cs still recognized which Add to use (context: compare Add above and Add below)
        public IActionResult Add(AddTagRequest addTagRequest)
        {
            // Mapping AddTagRequest to Tag to domain model
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName,
            };
            
            // get data from tag
            bloggieDbContext.Tags.Add(tag);
            // save changes
            bloggieDbContext.SaveChanges();

            return View("Add");
        }

        // DISPLAY functionality
        [HttpGet]
        public IActionResult List()
        {
            // get data from bloggieDbContext
            var tags = bloggieDbContext.Tags.ToList();

            // return data to view
            return View(tags);
        }   
    }
}
