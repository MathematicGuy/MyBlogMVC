using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyBlog.Web.Models.Domain;
using MyBlog.Web.Models.ViewModels;
using MyBlog.Web.Repository;
using System.Runtime.InteropServices;

namespace MyBlog.Web.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository tagResponsitory;
        private readonly IBlogPostRepository blogPostRepository;

        public AdminBlogPostsController(
            ITagRepository tagResponsitory,
            IBlogPostRepository blogPostRepository)
        {
            this.tagResponsitory = tagResponsitory;
            this.blogPostRepository = blogPostRepository;

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
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString()})
            };

            // return Tags (list item of tag) to Add View
            return View(model); 
        }

        // addBlogPostsRequest store input from Add View
        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostsRequest addBlogPostsRequest)
        {
            // Map View Model to Domain Model
            // ? Get input from Add View and store in BlogPost
            var blog = new BlogPost
            {
                Heading = addBlogPostsRequest.Heading,
                PageTitle = addBlogPostsRequest.PageTitle,
                Content = addBlogPostsRequest.Content,
                ShortDescription = addBlogPostsRequest.ShortDescription,
                FeatureImageIRL = addBlogPostsRequest.FeatureImageIRL,
                UrlHandle = addBlogPostsRequest.UrlHandle,
                PublishedDate = addBlogPostsRequest.PublishedDate,
                Author = addBlogPostsRequest.Author,
                Visible = addBlogPostsRequest.Visible,
            };

            // Map Tags from selected tags
            // ? Get selected tags from Add View and store in selectedTags
            var selectedTags = new List<Tag>();
            // Keep looping untill all selected tags are selected
            foreach (var selectedTagId in addBlogPostsRequest.SelectedTags)
            {

                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await tagResponsitory.GetByIdAsync(selectedTagIdAsGuid);

                // Add existingTag tags to selectedTags List
                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }

            }

            // Mapping tags back to domain model
            // Save selectedTags to BlogPost Tags List Item attributes
            blog.Tags = selectedTags;

            // Add BlogPost to Db
            await blogPostRepository.AddAsync(blog);

            // Redirect to Add View
            return RedirectToAction("Add");
        }

    }
}
