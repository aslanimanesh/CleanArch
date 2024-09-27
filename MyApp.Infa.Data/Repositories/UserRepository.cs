using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using MyApp.Domain.ViewModels.Users;
using MyApp.Infa.Data.Context;

namespace MyApp.Infa.Data.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        #region Fields
        private readonly MyAppDbContext _dbContext;
        #endregion

        #region Constructor
        public UserRepository(MyAppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Public Methods

        #region FilterAsync
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
            }));
            #endregion

            return model;
        }
        #endregion

        #region IsExistEmail
        public async Task<bool> IsExistEmail(string email, int? userId)
        {
            return _dbContext.Users.Any(u => u.Email == email && u.Id != userId);
        }
        #endregion

        #region IsExistUserName
        public async Task<bool> IsExistUserName(string userName , int? userId)
        {
            return _dbContext.Users.Any(u => u.UserName == userName && u.Id != userId);
        }
        #endregion

        #region LoginUser
        public async Task<User> LoginUser(LoginViewModel login)
        {
            string password = login.Password.Trim();
            string email = login.Email.Trim().ToLower();
            return _dbContext.Users.SingleOrDefault(u => u.Email == email && u.Password == password);
        }
        #endregion

        #endregion
    }
}
