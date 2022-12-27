using Nest;
using Search.API.Models;
using Polly;
using Elasticsearch.Net;

namespace Search.API.Extensions;

public static class ElasticsearchExtensions
{
    public static void AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
    {
        var url = configuration["ElasticConfiguration:Url"];
        var defaultIndex = configuration["ElasticConfiguration:DefaultIndex"];

        var settings = new ConnectionSettings(new Uri(url))
            .DefaultIndex(defaultIndex)
            .DefaultMappingFor<Product>(m => m.IndexName(defaultIndex));

        var client = new ElasticClient(settings);
        services.AddSingleton(client);

        try
        {
            var retry = Polly.Policy.Handle<ElasticsearchClientException>()
                .WaitAndRetry(new TimeSpan[]
                {
                    TimeSpan.FromSeconds(3),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(8),
                });

            retry.Execute(() => CreateIndex(client, defaultIndex));
        }
        catch (ElasticsearchClientException ex)
        {
            throw ex;
        }
    }

    private static void CreateIndex(ElasticClient client, string indexName)
    {
        var response = client.Indices.Create(indexName, c => c
            .Settings(s => s
                .NumberOfShards(1)
                .NumberOfReplicas(0)
                .Analysis(a => a
                    .Analyzers(an => an
                        .Custom("custom-analyzer", ca => ca
                            .Filters(new List<string> { "lowercase" })
                            .Tokenizer("whitespace")
                        )
                    )
                )
            )
            .Map<Product>(p => p
                .Dynamic(DynamicMapping.Strict)
                .AutoMap()
            )
        );
        if (!response.IsValid)
            throw new ElasticsearchClientException("Unable to create product index");
    }
}