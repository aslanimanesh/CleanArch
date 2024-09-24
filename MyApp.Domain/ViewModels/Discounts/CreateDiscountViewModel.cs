namespace MyApp.Domain.ViewModels.Discounts
{
    public class CreateDiscountViewModel
    {
        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DiscountCode { get; set; }
        public bool IsActive { get; set; }
    }
}
