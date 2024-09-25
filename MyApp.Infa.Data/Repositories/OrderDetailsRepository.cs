using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using MyApp.Infa.Data.Context;

namespace MyApp.Infa.Data.Repositories
{
    public class OrderDetailsRepository : GenericRepository<OrderDetail>, IOrderDetailsRepository
    {
        private readonly MyAppDbContext _dbContext;

        public OrderDetailsRepository(MyAppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OrderDetail> ExistProductInOrderDetail(int orderId, int productId)
        {
            var orderDetail = _dbContext.OrderDetails.FirstOrDefault(d => d.OrderId == orderId && d.ProductId == productId);
            return orderDetail;
        }

        public async Task<List<OrderDetail>> GetAllOrderDetailByOrderId(int orderId)
        {
            var orderDetails = await _dbContext.OrderDetails.Where(o=> o.OrderId == orderId).ToListAsync();
            return orderDetails;
        }
    }
}
