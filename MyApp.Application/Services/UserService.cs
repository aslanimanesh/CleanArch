using MyApp.Application.Interfaces;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using MyApp.Domain.ViewModels.Users;

namespace MyApp.Application.Services
{
    public class UserService : GenericService<User>, IUserService
    {

        #region Fields

        private readonly IUserRepository _userRepository;

        #endregion
       
        #region Constructor

        public UserService(IUserRepository userRepository) : base(userRepository)
        {
            _userRepository = userRepository;
        }

        #endregion

        #region User Filtering

        public async Task<FilterUserViewModel> FilterAsync(FilterUserViewModel model)
        {
            return await _userRepository.FilterAsync(model);
        }

        #endregion

        #region Existence Checks

        public async Task<bool> IsExistEmail(string email, int? userId)
        {
            return await _userRepository.IsExistEmail(email , userId);
        }

        public async Task<bool> IsExistUserName(string userName , int? userId)
        {
            return await _userRepository.IsExistUserName(userName , userId);
        }

        #endregion

        #region User Authentication

        public async Task<User> LoginUser(LoginViewModel login)
        {
            return await _userRepository.LoginUser(login);
        }

        #endregion

    }
}
