namespace MyBlog.Web.Models.ViewModels
{
    public class EditTagRequest
    {
        // Create new bc we want to seperate Edit Tag from the List Tag
        // Saving Edit Model data then transfer it to List Model
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
