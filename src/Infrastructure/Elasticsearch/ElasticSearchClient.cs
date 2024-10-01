using Elasticsearch.Net;
using Microsoft.Extensions.Options;
using Nest;

namespace Infrastructure.Elasticsearch;

public class ElasticSearchClient
{
    private readonly ElasticClient _client;

    public ElasticSearchClient(IOptions<ElasticSearchSettings> settings)
    {
        var node = new Uri(settings.Value.Url);
        var connectionPool = new SingleNodeConnectionPool(node);
        var connectionSettings = new ConnectionSettings(connectionPool);
        _client = new ElasticClient(connectionSettings);
    }

    public ElasticClient Client => _client;
}
