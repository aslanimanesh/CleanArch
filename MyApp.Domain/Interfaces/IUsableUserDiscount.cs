using MyApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Domain.Interfaces
{
    public interface IUsableUserDiscount : IGenericRepository<UsableUserDiscount>
    {
        Task<UsableUserDiscount> GetUsableUserDiscountAsync(int userId, int discountId);

    }
}
