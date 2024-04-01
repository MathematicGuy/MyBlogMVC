using Microsoft.EntityFrameworkCore;
using MyBlog.Web.Data;
using MyBlog.Web.Models.Domain;
using MyBlog.Web.Models.ViewModels;

namespace MyBlog.Web.Responsitory
{
    // inherit ITagResponsitory function
    public class TagResponsitory : ITagResponsitory
    {
        // Talk to the Db
        private readonly BloggieDbContext bloggieDbContext;

        // import BloggieDbContext from Program file so we can access the Db
        public TagResponsitory(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }
        
        public async Task<Tag> AddAsync(Tag tag)
        {
            // Want to talk to the Db
            await bloggieDbContext.Tags.AddAsync(tag);
            await bloggieDbContext.SaveChangesAsync();

            return tag;
        }

        // return all Tags Records from Db
        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await bloggieDbContext.Tags.ToListAsync();
        }

        public async Task<Tag> GetByIdAsync(Guid id)
        {
            if (id != null)
            {

            }
            return await bloggieDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            //var tag = await bloggieDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);

            var existingTag = await bloggieDbContext.Tags.FindAsync(tag.Id);

            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                // save changes
                await bloggieDbContext.SaveChangesAsync();

                return existingTag;
            }
            return null;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var existingTag = await bloggieDbContext.Tags.FindAsync(id);
            
            if (existingTag != null)    
            {
                bloggieDbContext.Tags.Remove(existingTag);
                await bloggieDbContext.SaveChangesAsync();
                return existingTag;
            }
            return null;
        }
    }
}
