using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using MyApp.Domain.ViewModels;
using MyApp.Infa.Data.Context;

namespace MyApp.Infa.Data.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly MyAppDbContext _dbContext;

        public UserRepository(MyAppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
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
