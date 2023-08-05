using ElasticSearchWithNet.Api.DTOs;
using ElasticSearchWithNet.Api.Models;
using Nest;

namespace ElasticSearchWithNet.Api.Repositories
{
    public class ProductRepository
    {
        private readonly ElasticClient _client;
        private const string productIndexName = "products";

        public ProductRepository(ElasticClient client)
        {
            _client = client;
        }

        public async Task<Product?> SaveAsync(Product product)
        {
            product.CreatedAt = DateTime.UtcNow;

            var response = await _client.IndexAsync(product, k => k.Index(productIndexName));
            if (!response.IsValid)
            {
                return null;
            }

            product.Id = response.Id;

            return product;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            var result = await _client.SearchAsync<Product>(k => k.Index(productIndexName).Query(x => x.MatchAll()));
            foreach (var hit in result.Hits)
            {
                hit.Source.Id = hit.Id;
            }

            return result.Documents.ToList();
        }

        public async Task<Product?> GetByIdAsync(string id)
        {
            var result = await _client.GetAsync<Product>(id, k => k.Index(productIndexName));
            if (!result.IsValid)
            {
                return null;
            }

            result.Source.Id = result.Id;   
            return result.Source;
        }

        public async Task<bool> UpdateAsync(ProductUpdateDto productToUpdate)
        {
            var result = await _client.UpdateAsync<Product, ProductUpdateDto>(productToUpdate.Id, k => k.Index(productIndexName).Doc(productToUpdate));
            return result.IsValid;
        }

        public async Task<DeleteResponse> DeleteAsync(string id)
        {
            var result = await _client.DeleteAsync<Product>(id, k => k.Index(productIndexName));
            return result;
        }
    }
}
