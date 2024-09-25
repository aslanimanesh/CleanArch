using MyApp.Application.Interfaces;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using MyApp.Domain.ViewModels;
using MyApp.Domain.ViewModels.Users;

namespace MyApp.Application.Services
{
    public class UserService : GenericService<User>, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) : base(userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<FilterUserViewModel> FilterAsync(FilterUserViewModel model)
        {
            return await _userRepository.FilterAsync(model);
        }

        public async Task<bool> IsExistEmail(string email)
        {
            return await _userRepository.IsExistEmail(email);
        }

        public async Task<bool> IsExistUserName(string userName)
        {
            return await (_userRepository.IsExistUserName(userName));
        }

        public async Task<User> LoginUser(LoginViewModel login)
        {
            return await _userRepository.LoginUser(login);
        }
    }
}

