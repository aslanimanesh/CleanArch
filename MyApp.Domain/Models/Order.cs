using MyApp.Domain.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace MyApp.Domain.Models
{
    public class Order : BaseEntity
    {

        #region Properties

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public decimal Sum { get; set; }

        public bool IsFinaly { get; set; }

        #endregion

        #region Navigation Properties

        public List<OrderDetail> OrderDetails { get; set; }

        #endregion

    }
}
