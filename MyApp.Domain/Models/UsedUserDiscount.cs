namespace MyApp.Domain.Models
{
    public class UsedUserDiscount
    {

        #region Properties

        public int Id { get; set; }
        public int UserId { get; set; }          
        public int DiscountId { get; set; }      
        public int OrderId { get; set; }         
        public DateTime UsedDate { get; set; }

        #endregion

        #region Navigation Properties

        public User User { get; set; }
        public Discount Discount { get; set; }
        public Order Order { get; set; }

        #endregion

    }
}
