namespace MyBlog.Web.Repository
{
    public interface IImageRepository
    {
        // get and return the Image Url to the Cloud
        Task<string> UploadAsync(IFormFile file);

    }
}
