using MyApp.Domain.Models;
using MyApp.Domain.ViewModels.AssignDiscount;

namespace MyApp.Application.Interfaces
{
    public interface IDiscountAssignmentToUserService : IGenericService<UserDiscount>
    {

        #region Discount Assignment

        Task AssignDiscountToUsersAsync(AssignDiscountToUserViewModel model);
        Task<UserDiscount> GetUserDiscountAsync(int userId, int discountId);

        #endregion

    }
}
