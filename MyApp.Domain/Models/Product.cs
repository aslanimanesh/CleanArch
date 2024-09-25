using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyApp.Domain.Models.Common;

namespace MyApp.Domain.Models
{
    public class Product : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(200)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string? ImageName { get; set; } = null;


        // Many-to-many relationship with Discount
        public ICollection<ProductDiscount> ProductDiscounts { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
    

}
