using MyApp.Domain.Models;
using MyApp.Domain.ViewModels;

namespace MyApp.Domain.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<FilterUserViewModel> FilterAsync(FilterUserViewModel model);
    }
}
