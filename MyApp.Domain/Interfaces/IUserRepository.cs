using MyApp.Domain.Models;
using MyApp.Domain.ViewModels;
using MyApp.Domain.ViewModels.Users;

namespace MyApp.Domain.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<FilterUserViewModel> FilterAsync(FilterUserViewModel model);
        Task<User> LoginUser(LoginViewModel login);
        Task<bool> IsExistUserName(string userName);
        Task<bool> IsExistEmail(string email);
    }
}
