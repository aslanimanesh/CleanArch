using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using MyApp.Domain.Models.Common;

namespace MyApp.Domain.Models
{
    public class Discount :  BaseEntity
    {
        [Required]
        [Range(0, 100)]

        public decimal DiscountPercentage { get; set; }

        public DateTime? StartDate { get; set; } = null;

        public DateTime? EndDate { get; set; } = null;

        [Required]
        [MaxLength(50)]
        public string DiscountCode { get; set; } 

        public bool IsActive { get; set; } = true;


        // Many-to-many relationship with Product
        public ICollection<ProductDiscount> ProductDiscounts { get; set; }

        // Many-to-many relationship with User
        public ICollection<UserDiscount> UserDiscounts { get; set; }

    }
}
