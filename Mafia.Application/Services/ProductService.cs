using Mafia.Core.Interfaces;
using Mafia.Core.Models;
using Mafia.Infrastructre;
using Microsoft.AspNetCore.Http;

namespace Mafia.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileRepository _fileRepository;

        public ProductService(IProductRepository productRepository, IFileRepository fileRepository)
        {
            _productRepository = productRepository;
            _fileRepository = fileRepository;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }


        public async Task<Product?> GetProductByIdAsync(string id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<string> CreateProductAsync(string name, string description, double price, int availableQuantity, string category, IFormFile image)
        {
            var productId = Guid.NewGuid().ToString();
            string imageUrl = "";
            if(image != null)
            {
                imageUrl = await _fileRepository.SaveProductImageAsync(productId, image);
            }
            var product = new Product
            {
                Id = productId,
                Name = name,
                Description = description,
                Price = price,
                AvailableQuantity = availableQuantity,
                Category = category,
                ImageUrl = imageUrl
            };
            return await _productRepository.CreateAsync(product);
        }

        public async Task<Product?> UpdateProductAsync(string id, string name, string description, double? price, int? availableQuantity, string category, IFormFile image)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if(product == null)
            {
                throw new Exception("Product not found");
            }
            if(image != null)
            {
                var imageUrl = await _fileRepository.SaveProductImageAsync(id, image);
                await _fileRepository.DeleteProductImageAsync(product.ImageUrl);
                product.ImageUrl = imageUrl;
            }
            if(name != null)
            {
                product.Name = name;
            }
            if(description != null)
            {
                product.Description = description;
            }
            if(price != null)
            {
                product.Price = price.Value;
            }
            if(availableQuantity != null)
            {
                product.AvailableQuantity = availableQuantity.Value;
            }
            if(category != null)
            {
                product.Category = category;
            }
            return await _productRepository.UpdateAsync(product);
        }

        public async Task<Product?> DeleteProductAsync(string id)
        {
            return await _productRepository.DeleteAsync(id);
        }

        public async Task<Product?> UpdateProductStockAsync(string id, int quantity)
        {
            return await _productRepository.UpdateStockAsync(id, quantity);
        }
    }
} 