using MyApp.Domain.Models;

namespace MyApp.Domain.ViewModels.Products
{
    public class ProductViewModelWithCalculateDiscount
    {
        public Product Product { get; set; }
        public decimal  DiscountPercentage  { get; set; }
        public decimal FinalPrice { get; set; }

    }
}
