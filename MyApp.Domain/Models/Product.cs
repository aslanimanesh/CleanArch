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

        // Navigation property for Discount (one-to-one relationship)
        public Discount Discount { get; set; }

        // Navigation property for Users (many-to-many relationship)
        public ICollection<User> Users { get; set; }
    }
    //محصول و یوزر 
    // تخفیف و یوزر

}
