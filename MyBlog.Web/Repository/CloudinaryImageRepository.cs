using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace MyBlog.Web.Repository
{
    public class CloudinaryImageRepository : IImageRepository
    {
        private readonly IConfiguration configuration;
        private readonly Account account;

        // config connection to Cloudinary
        public CloudinaryImageRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            account = new Account(
                configuration["Cloudinary:CloudName"],
                configuration["Cloudinary:ApiKey"],
                configuration["Cloudinary:ApiSecret"]);
        }


        // Upload Image to Cloudinary and return the Image connection URL
        public async Task<string> UploadAsync(IFormFile file)
        {
            var client = new Cloudinary(account);

            // uploadParams - image name and file
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                DisplayName = file.FileName,
            };

            // Upload Image File to  Cloudinary
            var uploadResult = await client.UploadAsync(uploadParams);
            
            // return Image URL (uploadResult) as string after successfully Uploaded
            if (uploadResult != null && uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUri.ToString();
            }
            else
            {
                return null;
            }
        }
    }
}
