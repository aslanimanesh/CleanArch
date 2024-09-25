using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using MyApp.Infa.Data.Context;

namespace MyApp.Infa.Data.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly MyAppDbContext _dbContext;

        public OrderRepository(MyAppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order> HasPendingOrder(int userId)
        {
            var order = _dbContext.Orders.SingleOrDefault(o => o.UserId == userId && !o.IsFinaly);
            return order;
        }
    }
}
