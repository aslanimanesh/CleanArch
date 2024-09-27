using MyApp.Domain.Models;
using MyApp.Domain.ViewModels.Users;

namespace MyApp.Application.Interfaces
{
    public interface IUserService : IGenericService<User>
    {
        #region User Filtering
        Task<FilterUserViewModel> FilterAsync(FilterUserViewModel model);
        #endregion

        #region User Authentication
        Task<User> LoginUser(LoginViewModel login);
        #endregion

        #region User Existence Check 
        Task<bool> IsExistUserName(string userName , int? userId);
        Task<bool> IsExistEmail(string email , int? userId);
        #endregion
    }
}
