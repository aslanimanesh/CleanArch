using MyApp.Domain.ViewModels.Products;

namespace MyApp.Domain.ViewModels.AssignDiscount
{
    public class AssignDiscountToProductViewModel
    {
        #region Properties
        public int DiscountId { get; set; }
        public List<int> ProductIds { get; set; } = new List<int>();
        public List<ProductViewModel> Products { get; set; } = new List<ProductViewModel>();

        #endregion
    }
}
