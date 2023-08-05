using ElasticSearchWithNet.Api.DTOs;
using ElasticSearchWithNet.Api.Models;
using ElasticSearchWithNet.Api.Repositories;
using Microsoft.Extensions.Logging;
using Nest;
using System.Net;

namespace ElasticSearchWithNet.Api.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ProductRepository productRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<ResponseDto<ProductDto>> SaveAsync(ProductCreateDto request)
        {
            var response = await _productRepository.SaveAsync(request.GetProduct());
            if (response == null)
            {
                return ResponseDto<ProductDto>.Fail(new List<string> { "hata oluştu" }, System.Net.HttpStatusCode.InternalServerError);
            }

            return ResponseDto<ProductDto>.Success(response.GetProductDto(), HttpStatusCode.Created);
        }

        public async Task<ResponseDto<List<ProductDto>>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            
            return ResponseDto<List<ProductDto>>.Success(products.Select(k => k.GetProductDto()).ToList(), HttpStatusCode.OK);
        }

        public async Task<ResponseDto<ProductDto>> GetByIdAsync(string id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return ResponseDto<ProductDto>.Fail(new List<string>() { $"Product not foud with id: {id}" }, HttpStatusCode.NotFound);
            }
            else
            {
                return ResponseDto<ProductDto>.Success(product.GetProductDto(), HttpStatusCode.OK);
            }
        }

        public async Task<ResponseDto<bool>> UpdateAsync(ProductUpdateDto productToUpdate)
        {
            var result = await _productRepository.UpdateAsync(productToUpdate);
            if (!result)
            {
                return ResponseDto<bool>.Fail(new List<string>() { $"An error accoured when updating the model" }, HttpStatusCode.InternalServerError);
            }
            else
            {
                return ResponseDto<bool>.Success(true, HttpStatusCode.NoContent);
            }
        }

        public async Task<ResponseDto<bool>> DeleteAsync(string id)
        {
            var deleteResponse = await _productRepository.DeleteAsync(id);
            if (!deleteResponse.IsValid && deleteResponse.Result == Result.NotFound)
            {
                return ResponseDto<bool>.Fail(new List<string>() { $"The product you want to delete can't found" }, HttpStatusCode.NotFound);
            }

            if (!deleteResponse.IsValid)
            {
                _logger.LogError(deleteResponse.OriginalException, deleteResponse.ServerError.ToString());

                return ResponseDto<bool>.Fail(new List<string>() { $"Error occured when deleting the product" }, HttpStatusCode.InternalServerError);
            }

            return ResponseDto<bool>.Success(true, HttpStatusCode.NoContent);
        }
    }
}
