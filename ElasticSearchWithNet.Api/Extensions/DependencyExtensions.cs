using Elasticsearch.Net;
using Nest;

namespace ElasticSearchWithNet.Api.Extensions
{
    public static class DependencyExtensions
    {
        public static void AddElastic(this IServiceCollection services, IConfiguration configuration)
        {
            var pool = new SingleNodeConnectionPool(new Uri(configuration["Elastic:Url"]!));
            var settings = new ConnectionSettings(pool);
            var client = new ElasticClient(settings);

            services.AddSingleton(client);
        }
    }
}
