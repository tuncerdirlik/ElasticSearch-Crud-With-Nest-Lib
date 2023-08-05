using ElasticSearchWithNet.Api.Models;

namespace ElasticSearchWithNet.Api.DTOs
{
    public record ProductUpdateDto(string Id, string Name, decimal Price, int Stock, ProductFeatureDto ProductFeature)
    {
        public Product GetProduct()
        {
            return new Product
            {
                Id = Id,
                Name = Name,
                Price = Price,
                Stock = Stock,
                Feature = new ProductFeature
                {
                    Color = ProductFeature.Color,
                    Height = ProductFeature.Height,
                    Width = ProductFeature.Width
                }
            };
        }
    }
}
