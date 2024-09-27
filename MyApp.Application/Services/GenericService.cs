using MyApp.Application.Interfaces;
using MyApp.Domain.Interfaces;

namespace MyApp.Application.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        #region Fields

        private readonly IGenericRepository<T> _repository;

        #endregion

        #region Constructor

        public GenericService(IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        #endregion

        #region CRUD Methods

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<T> AddAsync(T entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity != null)
            {
                await _repository.DeleteAsync(entity);
            }
        }

        #endregion
    }
}
