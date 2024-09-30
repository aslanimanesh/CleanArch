using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using MyApp.Infa.Data.Context;

namespace MyApp.Infa.Data.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        #region Fields

        private readonly MyAppDbContext _dbContext;

        #endregion

        #region Constructor

        public OrderRepository(MyAppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region Public Methods

        #region GetOrderWithDetailsAsync
        public async Task<Order> GetOrderWithDetailsAsync(int orderId)
        {
            return await _dbContext.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }
        #endregion

        #region HasPendingOrder
        public async Task<Order> HasPendingOrder(int userId)
        {
            return await _dbContext.Orders
                .FirstOrDefaultAsync(o => o.UserId == userId && !o.IsFinaly);
        }
        #endregion

        #region GetOrderTotalPriceAsync

        public async Task<decimal> GetOrderTotalPriceAsync(int orderId)
        {
            return await _dbContext.OrderDetails
                .Where(o => o.OrderId == orderId)
                .Select(d => d.Quantity * d.OriginalPrice)
                .SumAsync();
        }

        #endregion

        #region UpdatePaymentStatusAsync

        public async Task UpdatePaymentStatusAsync(int orderId)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.IsFinaly = true;
                _dbContext.Update(order);
                await _dbContext.SaveChangesAsync();
            }
        }

        #endregion

        #endregion
    }
}
