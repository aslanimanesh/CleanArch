using MyApp.Domain.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace MyApp.Domain.Models
{
    public class OrderDetail : BaseEntity
    {
        #region Properties

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        public decimal Price { get; set; }

        #endregion

        #region Navigation Properties

        public Order Order { get; set; }
        public Product Product { get; set; }

        #endregion
    }
}
