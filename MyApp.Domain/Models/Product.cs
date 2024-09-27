using MyApp.Domain.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace MyApp.Domain.Models
{
    public class Product : BaseEntity
    {
        #region Properties

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(200)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string? ImageName { get; set; } = null;

        #endregion

        #region Navigation Properties

        // Many-to-many relationship with Discount
        public ICollection<ProductDiscount> ProductDiscounts { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

        #endregion
    }
}
