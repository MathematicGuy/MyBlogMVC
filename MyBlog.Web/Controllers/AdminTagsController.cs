using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
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


        // CREATE Functionality
        // POST is used to send data to a server to create/update a resource.
        [HttpPost]
        //[ActionName("Add")] // if they're not the same Cs still recognized which Add to use (context: compare Add above and Add below)
        public IActionResult AddTag(AddTagRequest addTagRequest)
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

        [HttpPost]
        //[ActionName("SubmitTag")] // if they're not the same Cs still recognized which Add to use (context: compare Add above and Add below)

        public IActionResult SubmitTag(AddTagRequest submitTagRequest)
        {
            // Mapping AddTagRequest to Tag to domain model
            var tagg = new Tag
            {
                Name = submitTagRequest.Name,
                DisplayName = submitTagRequest.DisplayName,
            };

            // get data from tagg
            bloggieDbContext.Tags.Add(tagg);
            // save changes
            bloggieDbContext.SaveChanges();

            return RedirectToAction("List");
        }


        // DISPLAY Functionality
        [HttpGet]
        // use to redirect user to the List view 
        [ActionName("List")]
        public IActionResult List()
        {
            // get data from bloggieDbContext
            var tags = bloggieDbContext.Tags.ToList();

            // return data to view
            return View(tags);
        }

        [HttpGet]
        // Action Name is crucial bc it show What .cshtml file to render
        [ActionName("Edit")]
        // read Tag by id
        public IActionResult Edit(Guid id)
        {
            // 1st method 
            //var tag = bloggieDbContext.Tags.Find(id);
            
            // 2nd  method
            // Find tag by id and return the 1st one it found
            var tag = bloggieDbContext.Tags.FirstOrDefault(x => x.Id == id);  

            // Return EditTagRequest Model to the view if tag is not null
            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };

                return View(editTagRequest);
            }

            return View(null);
        }

        [HttpPost]
        // Action Name is crucial bc it show What .cshtml file to render
        [ActionName("Edit")]
        // notice: Tag.id != existingTag.id bc Tag.id is from EditTagRequest and existingTag.id is from bloggieDbContext
        // Edit - replace the existing tag with new tag data, then save changes
        public IActionResult Edit(EditTagRequest editTagRequest)
        {
            
              
            // tag object - get tag from editTagRequest (data from Edit.cshtml)
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName,
            };

            // existingTag object - get existing tag from bloggieDbContext (Data from Database) 
            var existingTag = bloggieDbContext.Tags.Find(tag.Id);
            
            // replace existing tag with new tag datas
            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                // save changes
                bloggieDbContext.SaveChanges();

                // show success notification
                TempData["SuccessMessage"] = "Tag updated successfully!";

                return RedirectToAction("List", new { id = editTagRequest.Id });
            }

            // show fail notification
            return RedirectToAction("List", new { id = editTagRequest.Id });
        }

        [HttpPost]
        [ActionName("Delete")]
        // Delete - remove tag from bloggieDbContext
        public IActionResult Delete(Guid Id)
        {
            // get tag by id
            var tag = bloggieDbContext.Tags.Find(Id);

            // remove tag from bloggieDbContext
            if (tag != null)
            {
                bloggieDbContext.Tags.Remove(tag);
                bloggieDbContext.SaveChanges();

                // show success notification
                TempData["SuccessMessage"] = "Tag deleted successfully!";
                return RedirectToAction("List");
            }

            // show fail notification
            TempData["ErrorMessage"] = "Failed to delete tag. Please try again."; 
            return RedirectToAction("Edit", new {id = Id});
        }
    }
}
