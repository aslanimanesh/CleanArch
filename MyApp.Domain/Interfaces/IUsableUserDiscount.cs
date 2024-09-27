using MyApp.Domain.Models;

namespace MyApp.Domain.Interfaces
{
    public interface IUsableUserDiscount : IGenericRepository<UsableUserDiscount>
    {
        #region Public Methods

        Task<UsableUserDiscount> GetUsableUserDiscountAsync(int userId, int discountId);

        #endregion
    }
}
