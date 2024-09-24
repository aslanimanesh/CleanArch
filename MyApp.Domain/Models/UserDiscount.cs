using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Domain.Models
{
    public class UserDiscount
    {
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }


        [ForeignKey("Discount")]
        public int DiscountId { get; set; }
        public Discount Discount { get; set; }
    }
}
