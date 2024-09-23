using System.ComponentModel.DataAnnotations;

namespace MyApp.Domain.Models
{
    public class Discount :  BaseEntity
    {
        [Required]
        [Range(0, 100)]
        public decimal DiscountPercentage { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string DiscountCode { get; set; }

        public bool IsActive => DateTime.Now >= StartDate && DateTime.Now <= EndDate;


        // Many-to-many relationship with Product
        public ICollection<ProductDiscount> ProductDiscounts { get; set; }

        // Many-to-many relationship with User
        public ICollection<UserDiscount> UserDiscounts { get; set; }

    }
}
