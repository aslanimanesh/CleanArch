using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Interfaces;
using MyApp.Infa.Data.Context;

namespace MyApp.Infa.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        #region Fields
        private readonly MyAppDbContext _dbContext;
        #endregion

        #region Constructor
        public GenericRepository(MyAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Public Methods

        #region GetAllAsync
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }
        #endregion

        #region GetByIdAsync
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        #endregion

        #region AddAsync
        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        #endregion

        #region UpdateAsync
        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        #endregion

        #region DeleteAsync
        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        #endregion

        #endregion
    }
}
