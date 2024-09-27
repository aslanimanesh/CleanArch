namespace MyApp.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        #region Public Methods

        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        #endregion
    }
}
