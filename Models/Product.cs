namespace WoodSalesPlatform.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? MainImage { get; set; } = string.Empty;
        public int Stock { get; set; }
        public bool IsFeatured { get; set; } = false;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    }
}
