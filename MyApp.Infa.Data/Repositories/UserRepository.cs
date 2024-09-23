using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using MyApp.Domain.ViewModels;
using MyApp.Infa.Data.Context;

namespace MyApp.Infa.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MyAppDbContext _dbContext;

        public UserRepository(MyAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<User> GetUsers()
        {
            return _dbContext.Users;
        }

        public User GetUserById(int UserId)
        {
            return _dbContext.Users.Find(UserId);
        }

        public bool CreateUser(User user)
        {
            _dbContext.Users.Add(user);
            return Save();
        }

        public bool UpdateUser(User user)
        {
            _dbContext.Users.Update(user);
            return Save();
        }

        public bool DeleteUser(int UserId)
        {
            var user = _dbContext.Users.Find(UserId);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                return Save();
            }
            return false;
        }

        private bool Save()
        {
            return _dbContext.SaveChanges() > 0;
        }

        public async Task<FilterUserViewModel> FilterAsync(FilterUserViewModel model)
        {
            var query = _dbContext.Users.AsQueryable();

            #region Filter 
            if (!string.IsNullOrEmpty(model.FirstName))
            {
                query = query.Where(user => user.FirstName == model.FirstName);
            }

            if (!string.IsNullOrEmpty(model.LastName))
            {
                query = query.Where(user => user.LastName == model.LastName);
            }
            #endregion

            #region Paging
            await model.Paging(query.Select(user => new UserDetailViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age
            }));
            #endregion

            return model;
        }
    }
}
