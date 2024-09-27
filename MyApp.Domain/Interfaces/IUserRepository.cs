using MyApp.Domain.Models;
using MyApp.Domain.ViewModels.Users;

namespace MyApp.Domain.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        #region User Management

        Task<FilterUserViewModel> FilterAsync(FilterUserViewModel model);
        Task<User> LoginUser(LoginViewModel login);

        #endregion

        #region User Validation

        Task<bool> IsExistUserName(string userName , int? userId);
        Task<bool> IsExistEmail(string email, int? userId);

        #endregion
    }
}
