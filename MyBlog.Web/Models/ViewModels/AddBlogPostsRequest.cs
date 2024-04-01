using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyBlog.Web.Models.ViewModels
{
    public class AddBlogPostsRequest
    {
        public string Heading { get; set; } 
        public string PageTitle { get; set; } 
        public string Content { get; set; } 
        public string ShortDescription { get; set; }
        public string FeatureImageIRL { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool Visible { get; set; }
        
        // Declare a List Item to hold Tags (Selected tags from Controller store in Tags List Item)
        public IEnumerable<SelectListItem> Tags { get; set; }
        // Selected Tag save here
        public string SelectedTag { get; set; }
    }   
}
