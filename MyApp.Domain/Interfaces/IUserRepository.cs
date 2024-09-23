using MyApp.Domain.Models;
using MyApp.Domain.ViewModels;

namespace MyApp.Domain.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();
        User GetUserById(int UserId);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(int UserId);
        Task<FilterUserViewModel> FilterAsync(FilterUserViewModel model);

   }
}
