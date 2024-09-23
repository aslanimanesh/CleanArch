using MyApp.Application.Interfaces;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using MyApp.Domain.ViewModels;

namespace MyApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserViewModel GetUsers()
        {
            return new UserViewModel
            {
                Users = _userRepository.GetUsers()
            };
        }

        public User GetUserById(int UserId)
        {
            User user = _userRepository.GetUserById(UserId);
            return user;
        }
        public bool CreateUser(User user)
        {
            return _userRepository.CreateUser(user);    
        }

        
        public bool UpdateUser(User user)
        {
            return _userRepository.UpdateUser(user);    
        }

      
        public bool DeleteUser(int UserId)
        {
            return _userRepository.DeleteUser(UserId); 
        }

        public async Task<FilterUserViewModel> FilterAsync(FilterUserViewModel model)
        {
            return await _userRepository.FilterAsync(model);
        }
    }
}

