using System.ComponentModel.DataAnnotations;

namespace MyApp.Domain.Models
{
    public class Product : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string ImageName { get; set; }


        // Many-to-many relationship with Discount
        public ICollection<ProductDiscount> ProductDiscounts { get; set; }
    }
    

}
