using Domain.Entities;

namespace Application.Abstract;

public interface IElasticsearchService
{
    Task<Post> IndexDocumentAsync(Post post);
    Task<Post> GetDocumentAsync(Guid id);
    Task<IEnumerable<Post>> SearchAsync(string query);
    Task UpdateDocumentAsync(Post post);
    Task DeleteDocumentAsync(Guid id);
}
