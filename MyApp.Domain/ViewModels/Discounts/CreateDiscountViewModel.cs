using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace MyApp.Domain.ViewModels.Discounts
{
    public class CreateDiscountViewModel
    {
        public decimal DiscountPercentage { get; set; }
        [AllowNull]
        public DateTime StartDate { get; set; }
        [AllowNull]
        public DateTime EndDate { get; set; }
        [AllowNull]
        public string DiscountCode { get; set; }
    
        public bool IsActive { get; set; }
    }
}
