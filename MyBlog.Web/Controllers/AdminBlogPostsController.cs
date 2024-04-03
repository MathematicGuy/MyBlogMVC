using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using MyBlog.Web.Models.Domain;
using MyBlog.Web.Models.ViewModels;
using MyBlog.Web.Repository;
using System.Runtime.InteropServices;

namespace MyBlog.Web.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;

        public AdminBlogPostsController(
            ITagRepository tagRepository,
            IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;

        }



        // Add tags to View 
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            // get tags from Db using function in repository
            var tags = await tagRepository.GetAllAsync();

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
        [ActionName("Add")]
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
                FeaturedImageUrl = addBlogPostsRequest.FeaturedImageUrl,
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
                // Convert input string into Guid id format 
                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await tagRepository.GetByIdAsync(selectedTagIdAsGuid);

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

        [HttpGet]
        //[ActionName("List")]
        public async Task<IActionResult> List()
        {
            // Call the repository 
            var blogs = await blogPostRepository.GetAllAsync();

            // return data to view
            return View(blogs);
        }

        // View EditedBlog Page
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            // Retrieve the resilt from the repository
            var blogPost = await blogPostRepository.GetAsync(id);
            var tagsDomainModel = await tagRepository.GetAllAsync();

            // map the domain model into the view model
            if (blogPost != null)
            {
                var editBlogPost = new EditBlogPostRequest
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    ShortDescription = blogPost.ShortDescription,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    PublishedDate = blogPost.PublishedDate,
                    Author = blogPost.Author,
                    Visible = blogPost.Visible,
                    // Select tag values from the list of tags
                    Tags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString(),
                    }),
                };

                // Tell what selected tag was
                // Show us that the selected tag was selected
                var selectedTags = blogPost.Tags.Select(
                    x => x.Id.ToString()).ToArray();

                return View(editBlogPost);
            };

            return View(null);
        }
        // Edit BlogPost by id
        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            // map view model back to domain model
            var blogPostDomainMode = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                ShortDescription = editBlogPostRequest.ShortDescription,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                UrlHandle = editBlogPostRequest.UrlHandle,
                PublishedDate = editBlogPostRequest.PublishedDate,
                Author = editBlogPostRequest.Author,
                Visible = editBlogPostRequest.Visible,
            };

            // Map tags into Domain Mode
            var selectedTags = new List<Tag>();
            foreach (var selectedTag in editBlogPostRequest.SelectedTags)
            {
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await tagRepository.GetByIdAsync(tag);
                    
                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }

            blogPostDomainMode.Tags = selectedTags;

            // Submit information to repository to update
            var updatedBlog = await blogPostRepository.UpdateAsync(blogPostDomainMode);
            if (updatedBlog != null)
            {
                // Show success notification
                return RedirectToAction("List");
            }

            // Show fail notification
            return RedirectToAction("Edit");

            // redirect to GET

        }

        // Id -> 00000000 bug solve (Cause: Delete Action get id from Add View Form. Not route id)
        // Delete BlogPost by id
        [HttpPost]
        // Delete - remove tag from bloggieDbContext
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            // use find for get existing tag
            // Talk to the repository
            var deletedBlogPost = await blogPostRepository.DeleteAsync(editBlogPostRequest.Id);
            if (deletedBlogPost != null)
            {
                // Show success notification
                return RedirectToAction("List");
            }
            // show fail notification
            TempData["ErrorMessage"] = "Failed to delete tag. Please try again.";

            return RedirectToAction("List", new { id = editBlogPostRequest.Id });
        }
    }
}
