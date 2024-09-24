using MyApp.Domain.Models;
using MyApp.Domain.ViewModels.AssignDiscount;

namespace MyApp.Application.Interfaces
{
    public interface IDiscountAssignmentToUserService: IGenericService<UserDiscount>
    {
        Task AssignDiscountToUsersAsync(AssignDiscountToUserViewModel model);
        Task<UserDiscount> GetUserDiscountAsync(int userId, int discountId);
    }
}
