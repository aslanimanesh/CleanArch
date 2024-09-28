using System.ComponentModel.DataAnnotations;

namespace MyApp.Domain.ViewModels.Discounts
{
    public class CreateDiscountViewModel
    {
        #region Properties

        [Required]
        [Range(0, 100)]
        public int DiscountPercentage { get; set; }

        public DateTime? StartDate { get; set; } = null;

        public DateTime? EndDate { get; set; } = null;

        public string? DiscountCode { get; set; } = null;

        [Required]
        public int UsableCount { get; set; }

        public bool IsActive { get; set; } = true;

        #endregion
    }
}
