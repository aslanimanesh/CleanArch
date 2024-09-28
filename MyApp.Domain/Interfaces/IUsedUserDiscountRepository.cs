using MyApp.Domain.Models;

namespace MyApp.Domain.Interfaces
{
    public interface IUsedUserDiscountRepository : IGenericRepository<UsedUserDiscount>
    {
        Task<UsedUserDiscount> FindUsedUserDiscountByCodeAsync(int userId, string discountCode);

    }
}
