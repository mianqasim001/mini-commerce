using backend.DTOs;
using backend.Repositories;
using backend.Models;           // Required for the 'Product' entity
using Microsoft.AspNetCore.Http; // Required for 'IFormFile'
using Microsoft.AspNetCore.Hosting; // Required for 'IWebHostEnvironment'
using System.Linq;
using System.IO;                // Required for Path and File operations

namespace backend.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _environment;

        public ProductService(IProductRepository productRepository, IWebHostEnvironment environment)
        {
            _productRepository = productRepository;
            _environment = environment;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(p => new ProductDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                ImagePath = p.ImagePath,
                CategoryName = p.Category?.CategoryName ?? "Uncategorized"
            });
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            return new ProductDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                ImagePath = product.ImagePath,
                CategoryName = product.Category?.CategoryName ?? "Uncategorized"
            };
        }

        public async Task<Product> CreateProductAsync(ProductCreateDto dto, IFormFile? imageFile)
        {
            Console.WriteLine(dto.ProductName + " " + dto.Description + " " + dto.Price + " " + dto.StockQuantity + " " + dto.ImageFile);
            string imagePath = "default.jpg";

            if (imageFile != null && imageFile.Length > 0)
            {
                imagePath = await SaveImageAsync(imageFile);
            }

            var product = new Product
            {
                ProductName = dto.ProductName,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                ImagePath = imagePath,
                CategoryId = dto.CategoryId // ✅ Add this
            };

            return await _productRepository.AddAsync(product);
        }

        private async Task<string> SaveImageAsync(IFormFile file)
        {
            // Path: backend/wwwroot/uploads
            string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return "/uploads/" + fileName; // This is what goes in the DB
        }

        public async Task UpdateProductAsync(int id, ProductUpdateDto dto)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) throw new Exception("Product not found");

            product.ProductName = dto.ProductName;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.StockQuantity = dto.StockQuantity;

            // Handle image replacement if a new file was uploaded
            if (dto.ImageFile != null && dto.ImageFile.Length > 0)
            {
            // Optional: Add logic to delete the old physical file here if needed

            // Save the new image using your existing SaveImageAsync method
               product.ImagePath = await SaveImageAsync(dto.ImageFile);
            }

            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteAsync(id);
        }
    }
}