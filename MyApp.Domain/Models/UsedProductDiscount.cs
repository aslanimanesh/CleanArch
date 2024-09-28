namespace MyApp.Domain.Models
{
    public class UsedProductDiscount
    {
        #region Properties

        public int Id { get; set; }
        public int ProductId { get; set; }        // شناسه محصول
        public int DiscountId { get; set; }       // شناسه تخفیف
        public int OrderId { get; set; }          // شناسه سفارش
        public DateTime UsedDate { get; set; }    // تاریخ استفاده

        // روابط
        public Product Product { get; set; }
        public Discount Discount { get; set; }
        public Order Order { get; set; }

        #endregion
    }
}
