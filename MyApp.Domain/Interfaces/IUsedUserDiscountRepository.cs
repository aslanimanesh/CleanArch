using MyApp.Domain.Models;

namespace MyApp.Domain.Interfaces
{
    public interface IUsedUserDiscountRepository : IGenericRepository<UsedUserDiscount>
    {

        #region FindUsedUserDiscountByCode

        Task<UsedUserDiscount> FindUsedUserDiscountByCodeAsync(int userId, string discountCode);

        #endregion

    }
}
