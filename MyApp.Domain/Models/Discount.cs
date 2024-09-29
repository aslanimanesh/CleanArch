using MyApp.Domain.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace MyApp.Domain.Models
{
    public class Discount : BaseEntity
    {

        #region Properties

        [Required]
        [Range(0, 100)]
        public int DiscountPercentage { get; set; }

        public DateTime? StartDate { get; set; } = null;

        public DateTime? EndDate { get; set; } = null;

        public string? DiscountCode { get; set; } = null;
 
        public int? UsableCount { get; set; } = null;

        public int? UsedCount { get; set; } = null;

        public bool IsGeneralForProducts { get; set; }=false;

        public bool IsGeneralForUsers { get; set; } =false;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        #endregion

        #region Navigation Properties

        // Many-to-many relationship with Product
        public ICollection<ProductDiscount> ProductDiscounts { get; set; }

        // Many-to-many relationship with User
        public ICollection<UserDiscount> UserDiscounts { get; set; }

        #endregion

    }
}
