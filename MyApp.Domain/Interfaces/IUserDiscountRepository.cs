using MyApp.Domain.Models;
using System.Linq.Expressions;

namespace MyApp.Domain.Interfaces
{
    public interface IUserDiscountRepository : IGenericRepository<UserDiscount>
    {
        Task<UserDiscount> GetAsync(Expression<Func<UserDiscount, bool>> predicate);
        Task<List<UserDiscount>> GetDiscountsForUserAsync(int userId);
    }
}
