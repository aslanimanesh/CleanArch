using MyApp.Domain.Models;
using MyApp.Domain.ViewModels;

namespace MyApp.Application.Interfaces
{
    public interface IUserService : IGenericService<User>
    {
        Task<FilterUserViewModel> FilterAsync(FilterUserViewModel model);
    }
}
