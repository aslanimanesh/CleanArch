namespace MyApp.Domain.Models
{
    public class UsedUserDiscount
    {
        #region Properties
        public int Id { get; set; }
        public int UserId { get; set; }          // شناسه کاربر
        public int DiscountId { get; set; }       // شناسه تخفیف
        public int OrderId { get; set; }          // شناسه سفارش مرتبط با تخفیف
        public DateTime UsedDate { get; set; }    // تاریخ استفاده

        // روابط
        public User User { get; set; }
        public Discount Discount { get; set; }
        public Order Order { get; set; }

        #endregion
    }
}
