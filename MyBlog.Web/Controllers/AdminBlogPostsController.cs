using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyBlog.Web.Models.ViewModels;
using MyBlog.Web.Responsitory;

namespace MyBlog.Web.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagResponsitory tagResponsitory;

        public AdminBlogPostsController(ITagResponsitory tagResponsitory)
        {
            this.tagResponsitory = tagResponsitory;
        }

        // Add tags to View 
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            // get tags from Db using function in repository
            var tags = await tagResponsitory.GetAllAsync();

            var model = new AddBlogPostsRequest
            {
                // Select from Tag Domain Model to Tags in AddBlogPostsRequest Model 
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };

            // return Tags (list item of tag) to Add View
            return View(model); 
        }

        // addBlogPostsRequest store input from Add View
        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostsRequest addBlogPostsRequest)
        {
            return RedirectToAction("Add");
        }

    }
}
