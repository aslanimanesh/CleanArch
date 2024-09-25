using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyApp.Domain.Models.Common;

namespace MyApp.Domain.Models
{
    public class Order : BaseEntity
    {
        
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
