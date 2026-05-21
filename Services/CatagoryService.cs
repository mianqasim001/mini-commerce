using backend.Data;
using backend.DTOs;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace backend.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();

            return categories.Select(c => new CategoryDto
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                CategoryType = c.CategoryType // Mapping the new column
            });
        }

        public async Task<CategoryWithProductsDto> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null) throw new Exception("Category not found");

            return new CategoryWithProductsDto
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                CategoryType = category.CategoryType,
                Products = category.Products.Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CategoryName = category.CategoryName,
                    ImagePath = p.ImagePath
                }).ToList()
            };
        }

        public async Task<Category> CreateCategoryAsync(CategoryCreateDto dto)
        {
            var category = new Category
            {
                CategoryName = dto.CategoryName,
                CategoryType = dto.CategoryType
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

//         public async Task<IEnumerable<CategoryDto>> CreateCategoriesBulkAsync(List<CategoryCreateDto> dtos)
// {
//     // Map the DTOs into Model entities
//     var categoriesToCreate = dtos.Select(dto => new Category
//     {
//         CategoryName = dto.CategoryName,
//         CategoryType = dto.CategoryType
//     }).ToList();

//     // Add everything to the tracking context at once
//     await _context.Categories.AddRangeAsync(categoriesToCreate);
//     await _context.SaveChangesAsync();

//     // Return the newly created items mapped back to CategoryDto (including their new IDs)
//     return categoriesToCreate.Select(c => new CategoryDto
//     {
//         CategoryId = c.CategoryId,
//         CategoryName = c.CategoryName,
//         CategoryType = c.CategoryType
//     });
// }

        public async Task UpdateCategoryAsync(int id, CategoryUpdateDto dto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) throw new Exception("Category not found");

            category.CategoryName = dto.CategoryName;
            category.CategoryType = dto.CategoryType;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) throw new Exception("Category not found");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}