using System.ComponentModel.DataAnnotations;

namespace MyApp.Domain.ViewModels.Discounts
{
    public class EditDiscountViewModel
    {
        #region Properties

        public int Id { get; set; }

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
    }
}
