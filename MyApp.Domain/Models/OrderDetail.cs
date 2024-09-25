using System.ComponentModel.DataAnnotations;
using MyApp.Domain.Models.Common;

namespace MyApp.Domain.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailID { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        public decimal Price { get; set; }



        public Order Order { get; set; }
        public Product Product { get; set; }

    }
}
