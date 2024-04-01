using Microsoft.AspNetCore.Mvc.Rendering;

using MyBlog.Web.Data;
using MyBlog.Web.Models.Domain;

namespace MyBlog.Web.Repository
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public BlogPostRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }
        
        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            // Take BlogPost Model as input and add to Db
            await bloggieDbContext.BlogPosts.AddAsync(blogPost);
            await bloggieDbContext.SaveChangesAsync();
            
            return blogPost;
        }

        public async Task<BlogPost> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<BlogPost> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<BlogPost> UpdateAsync(BlogPost blogPost)
        {
            throw new NotImplementedException();
        }
    }
}
