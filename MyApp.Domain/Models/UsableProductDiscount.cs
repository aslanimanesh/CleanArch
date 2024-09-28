namespace MyApp.Domain.Models
{
    public class UsableProductDiscount
    {
        #region Properties

        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int DiscountId { get; set; }

        #endregion
    }
}
