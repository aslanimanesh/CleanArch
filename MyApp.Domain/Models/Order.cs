using System.ComponentModel.DataAnnotations;
using MyApp.Domain.Models.Common;

namespace MyApp.Domain.Models
{
    public class Order 
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public decimal Sum { get; set; }

        public bool IsFinaly { get; set; }



        public List<OrderDetail> OrderDetails { get; set; }
    }
}
