using System.ComponentModel.DataAnnotations.Schema;

namespace WoodSalesPlatform.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; } = string.Empty;

        public bool IsPrimary { get; set; } = false;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
