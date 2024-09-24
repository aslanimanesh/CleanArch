using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Domain.Models
{
    public class ProductDiscount
    {
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }


        [ForeignKey("Discount")]
        public int DiscountId { get; set; }
        public Discount Discount { get; set; }
    }
}
