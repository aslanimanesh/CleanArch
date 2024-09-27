using MyApp.Domain.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace MyApp.Domain.Models
{
    public class Discount : BaseEntity
    {
        #region Properties

        [Required]
        [Range(0, 100)]
        public decimal DiscountPercentage { get; set; }

        public DateTime? StartDate { get; set; } = null;

        public DateTime? EndDate { get; set; } = null;

        [Required]
        [MaxLength(50)]
        public string DiscountCode { get; set; }

        [Required]
        public int UsableCount { get; set; }

        public bool IsActive { get; set; } = true;

        #endregion

        #region Navigation Properties

        // Many-to-many relationship with Product
        public ICollection<ProductDiscount> ProductDiscounts { get; set; }

        // Many-to-many relationship with User
        public ICollection<UserDiscount> UserDiscounts { get; set; }

        #endregion
    }
}
