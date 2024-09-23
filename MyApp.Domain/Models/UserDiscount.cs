using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Domain.Models
{
    public class UserDiscount
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int DiscountId { get; set; }
        public Discount Discount { get; set; }
    }
}
