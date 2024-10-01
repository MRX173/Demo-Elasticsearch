using Application.Abstract;
using Domain.Entities;
using Nest;

namespace Infrastructure.Elasticsearch.Services;

public class ElasticsearchService : IElasticsearchService
{
    private readonly IElasticClient _client;
    private readonly string _indexName;

    public ElasticsearchService(IElasticClient elastic, ElasticSearchSettings settings)
    {
        _client = elastic;
        _indexName = settings.DefaultIndex;
    }

    public async Task DeleteDocumentAsync(Guid id)
    {
        var response = await _client.DeleteAsync<Post>(id, d => d.Index(_indexName));
        if (!response.IsValid)
        {
            throw new Exception("failed to Delete document");
        }
    }

    public async Task<Post> GetDocumentAsync(Guid id)
    {
        var response = await _client.GetAsync<Post>(id, idx => idx.Index(_indexName));
        if (!response.IsValid)
        {
            throw new Exception("failed to get document");
        }
        return response.Source;
    }

    public async Task<Post> IndexDocumentAsync(Post post)
    {
        var response = await _client.IndexDocumentAsync(post);
        if (!response.IsValid)
        {
            throw new Exception("faild to index document");
        }
        return post;
    }

    public async Task<IEnumerable<Post>> SearchAsync(string query)
    {
        var response = await _client.SearchAsync<Post>(s =>
            s.Index(_indexName)
                .Query(q =>
                    q.MultiMatch(mm =>
                        mm.Fields(f => f.Field(p => p.Title).Field(p => p.Content))
                            .Query(query)
                            .Type(TextQueryType.BestFields)
                            .Fuzziness(Fuzziness.Auto)
                    )
                )
        );
        if (!response.IsValid)
        {
            throw new Exception("failed to Search document");
        }

        return response.Documents;
    }

    public async Task UpdateDocumentAsync(Post post)
    {
        var response = await _client.UpdateAsync<Post>(post.Id, u => u.Index(_indexName).Doc(post));
        if (!response.IsValid)
        {
            throw new Exception("failed to Update document");
        }
    }
}
