using ElasticSearchWithNet.Api.DTOs;
using Nest;

namespace ElasticSearchWithNet.Api.Models
{
    public class Product
    {
        [PropertyName("_id")]
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ProductFeature? Feature { get; set; }

        public ProductDto GetProductDto()
        {
            if (Feature == null)
            {
                return new ProductDto(Id, Name, Price, Stock, null);
            }

            return new ProductDto(Id, Name, Price, Stock, new ProductFeatureDto(Feature.Width, Feature.Height, Feature.Color));
        }
    }
}
