using Microsoft.AspNetCore.Mvc;
using MyBlog.Web.Models.Domain;
using MyBlog.Web.Models.ViewModels;
using MyBlog.Web.Repository;

namespace MyBlog.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository tagResponsitory;

        public AdminTagsController(ITagRepository tagResponsitory)
        {
            this.tagResponsitory = tagResponsitory;
        }

        // GET: /AdminTags/Add
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }


        // CREATE Functionality
        // POST is used to send data to a server to create/update a resource.
        [HttpPost]
        //[ActionName("Add")] // if they're not the same Cs still recognized which Add to use (context: compare Add above and Add below)
        public async Task<IActionResult> AddTag(AddTagRequest addTagRequest)
        {
            // Mapping AddTagRequest to Tag to domain model
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName,
            };

            // use await to make it complete async
            // do all these function async
            await tagResponsitory.AddAsync(tag);

            return View("Add");
        }

        [HttpPost]
        //[ActionName("SubmitTag")] // if they're not the same Cs still recognized which Add to use (context: compare Add above and Add below)

        public async Task<IActionResult> SubmitTag(AddTagRequest submitTagRequest)
        {
            // Mapping AddTagRequest to Tag to domain model
            var tag = new Tag
            {
                Name = submitTagRequest.Name,
                DisplayName = submitTagRequest.DisplayName,
            };

            // inject the respotory from TagResponsitory
            await tagResponsitory.AddAsync(tag);


            return RedirectToAction("List");
        }


        // DISPLAY Functionality
        [HttpGet]
        // use to redirect user to the List view 
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            // get data from bloggieDbContext
            var tags = await tagResponsitory.GetAllAsync();

            // return data to view
            return View(tags);
        }

        [HttpGet]
        // read Tag by id
        public async Task<IActionResult> Edit(Guid id)
        {
            // 1st method 
            //var tag = bloggieDbContext.Tags.Find(id);
            
            // 2nd  method
            // Use FirstOrDefault to Find tag by id and return the 1st one it found
            var tag = await tagResponsitory.GetByIdAsync(id);  

            // Return EditTagRequest Model to the view if tag is not null
            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName,
                };

                return View(editTagRequest);
            }

            return View(null);
        }

        // Method Overloading allow 2 Edit Actions not get Conficted.
        [HttpPost]
        // Edit - replace the existing tag with new tag data
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            // Notice: tag.id != existingTag.id 
            // tag object - get tag from editTagRequest (data from Edit.cshtml)
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName,
            };

            // existingTag object - get existing tag from bloggieDbContext (Data from Database) 
            var updatedTag = await tagResponsitory.UpdateAsync(tag);
            
            if (updatedTag != null)
            {
                // Show success notification
            }
            else
            {
                // show fail notification

            }
            return RedirectToAction("List", new { id = editTagRequest.Id });
        }

        [HttpPost]
        // Delete - remove tag from bloggieDbContext
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            // get tag by id
            // use find for existing tag id
            var deletedTag = await tagResponsitory.DeleteAsync(editTagRequest.Id);
            if (deletedTag != null)
            {
                // Show success notification
            }
            else
            {
                // show fail notification
                TempData["ErrorMessage"] = "Failed to delete tag. Please try again.";
            }
            return RedirectToAction("List", new { id = editTagRequest.Id });
        }
    }
}
