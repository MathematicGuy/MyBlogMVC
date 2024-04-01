using MyBlog.Web.Models.Domain;

namespace MyBlog.Web.Responsitory
{
    // interface for Tag. Just include the Definition for Functionality
    public interface ITagResponsitory
    {
        // DB always know about the DOMAIN MODEL
        Task<IEnumerable<Tag>> GetAllAsync();
        Task<Tag> GetByIdAsync(Guid id);
        Task<Tag> AddAsync(Tag tag);
        Task<Tag?> UpdateAsync(Tag tag);
        Task<Tag?> DeleteAsync(Guid id);
    }
}
