using MyBlog.Web.Models.Domain;

namespace MyBlog.Web.Models.ViewModels
{
    public class AddTagRequest
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
