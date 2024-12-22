using System.ComponentModel.DataAnnotations;
using WarehouseWebsite.Domain.Models.Items;

namespace WarehouseWebsite.Web.Models
{
    public class ItemRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; } = null!;

        public int Quantity { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 10)]
        public string Description { get; set; } = null!;

        [Range(0.01, 10_000_000)]
        public decimal Price { get; set; }

        [Range(0.1, 1000)]
        public double Weight { get; set; }

        public ItemCategory Category { get; set; }

        [Required]
        public IFormFile Image { get; set; } = null!;

        public Item GetItem()
        {
            return new()
            {
                Name = Name,
                Quantity = Quantity,
                Description = Description,
                Price = Price,
                Weight = Weight,
                Category = Category
            };
        }

        public async Task<Stream> GetStreamAsync()
        {
            var memoryStream = new MemoryStream();
            await Image.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
