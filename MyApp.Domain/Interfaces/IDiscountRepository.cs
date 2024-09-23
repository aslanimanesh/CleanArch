using MyApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Domain.Interfaces
{
    public interface IDiscountRepository
    {
        IEnumerable<Discount> GetDiscounts();
        Discount GetDiscountById(int DiscountId);
        bool CreateDiscount(Discount discount);
        bool UpdateDiscount(Discount discount);
        bool DeleteDiscount(int discountId);
    }
}
