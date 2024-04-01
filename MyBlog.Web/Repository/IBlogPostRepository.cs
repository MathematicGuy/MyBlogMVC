using MyBlog.Web.Models.Domain;

namespace MyBlog.Web.Repository
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<BlogPost>> GetAllAsync();
        Task<BlogPost> GetAsync(Guid id);
        Task<BlogPost> AddAsync(BlogPost blogPost);
        Task<BlogPost> UpdateAsync(BlogPost blogPost);
        Task<BlogPost> DeleteAsync(Guid id);
    }
}
