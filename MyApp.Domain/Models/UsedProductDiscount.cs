namespace MyApp.Domain.Models
{
    public class UsedProductDiscount
    {

        #region Properties

        public int Id { get; set; }
        public int ProductId { get; set; }       
        public int DiscountId { get; set; }       
        public int OrderId { get; set; }          
        public DateTime UsedDate { get; set; }

        #endregion

        #region Navigation Properties

        public Product Product { get; set; }
        public Discount Discount { get; set; }
        public Order Order { get; set; }

        #endregion

    }
}
