using MyApp.Application.Interfaces;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using MyApp.Domain.ViewModels.AssignDiscount;

namespace MyApp.Application.Services
{
    public class DiscountAssignmentToUserService : GenericService<UserDiscount>, IDiscountAssignmentToUserService
    {
        private readonly IUserDiscountRepository _userDiscountRepository;

        public DiscountAssignmentToUserService(IUserDiscountRepository userDiscountRepository) : base(userDiscountRepository)
        {
            _userDiscountRepository = userDiscountRepository;
        }

        public async Task AssignDiscountToUsersAsync(AssignDiscountToUserViewModel model)
        {
            var userDiscounts = model.UserIds.Select(userId => new UserDiscount
            {
                UserId = userId,
                DiscountId = model.DiscountId
            });

            foreach (var ur in userDiscounts)
            {
                await _userDiscountRepository.AddAsync(ur);
            }
        }

        public async Task<UserDiscount> GetUserDiscountAsync(int userId, int discountId)
        {
            return await _userDiscountRepository.GetAsync(pd => pd.UserId == userId && pd.DiscountId == discountId);
        }
    }
}
