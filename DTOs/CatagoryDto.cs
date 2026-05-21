namespace backend.DTOs
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryType { get; set; } = string.Empty;
    }

    public class CategoryCreateDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryType { get; set; } = string.Empty;
    }

    public class CategoryUpdateDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryType { get; set; } = string.Empty;
    }

    public class CategoryWithProductsDto : CategoryDto
    {
        public List<ProductDto> Products { get; set; } = new();
    }
}