using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MyBlog.Web.Models.Domain;
namespace MyBlog.Web.Data
{
    // BloggieDbContext Inherit from DbContext
    public class BloggieDbContext : DbContext
    {
        // Overwrite DbContextOptions with BloggieDbContext
        public BloggieDbContext(DbContextOptions<BloggieDbContext> options) : base(options)
        {

        }

        // BlogPost as DbSet
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Tag> Tags { get; set; }

    }
}
