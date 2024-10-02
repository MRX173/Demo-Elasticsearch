using Domain.Entities;

namespace Application.Abstract;

public interface IPostRepository
{
    Task<Post> GetByIdAsync(Guid id);
    Task<List<Post>> GetAllAsync();
    Task<Post> AddAsync(Post post);
    Task UpdateAsync(Post post);
    Task DeleteAsync(Guid id);
}
