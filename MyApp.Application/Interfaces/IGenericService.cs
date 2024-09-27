namespace MyApp.Application.Interfaces
{
    public interface IGenericService<T> where T : class
    {
        #region CRUD Operations
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        #endregion
    }
}
