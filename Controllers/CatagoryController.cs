using backend.Services;
using backend.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            try 
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                return Ok(categories);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(categoryId);
                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody]CategoryCreateDto dto)
        {
            try
            {
                var category = await _categoryService.CreateCategoryAsync(dto);
                return Ok(new { message = "Category created successfully", category });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{categoryId}")]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] CategoryUpdateDto dto)
        {
            try
            {
                await _categoryService.UpdateCategoryAsync(categoryId, dto);
                return Ok(new { message = "Category updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(categoryId);
                return Ok(new { message = "Category deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

//         [HttpPost("bulk")]
// public async Task<IActionResult> CreateCategoriesBulk([FromBody] List<CategoryCreateDto> dtos)
// {
//     try
//     {
//         if (dtos == null || !dtos.Any())
//         {
//             return BadRequest(new { message = "Category list cannot be empty." });
//         }

//         var createdCategories = await _categoryService.CreateCategoriesBulkAsync(dtos);
//         return Ok(new { message = "All categories created successfully", categories = createdCategories });
//     }
//     catch (Exception ex)
//     {
//         return BadRequest(new { message = ex.Message });
//     }
// }
    }
}