using Microsoft.EntityFrameworkCore;
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

        //public DiscountUseType UseDiscount(int orderId, string code)
        //{
        //    var discount = _dbContext.Discounts.SingleOrDefault(d => d.DiscountCode == code);

        //    if (discount == null)
        //        return DiscountUseType.NotFound;

        //    if (discount.StartDate != null && discount.StartDate < DateTime.Now)
        //        return DiscountUseType.ExpierDate;

        //    if (discount.EndDate != null && discount.EndDate >= DateTime.Now)
        //        return DiscountUseType.ExpierDate;


        //    if (discount.UsableCount != null && discount.UsableCount < 1)
        //        return DiscountUseType.Finished;

        //    var order = GetByIdAsync(orderId);

        //    if (_dbContext.UserDiscountCodes.Any(d => d.UserId == order.UserId && d.DiscountId == discount.DiscountId))
        //        return DiscountUseType.UserUsed;

        //    int percent = (order.OrderSum * discount.DiscountPercent) / 100;
        //    order.OrderSum = order.OrderSum - percent;

        //    UpdateOrder(order);

        //    if (discount.UsableCount != null)
        //    {
        //        discount.UsableCount -= 1;
        //    }

        //    _dbContext.Discounts.Update(discount);
        //    _dbContext.UserDiscountCodes.Add(new UserDiscountCode()
        //    {
        //        UserId = order.Id,
        //        DiscountId = discount.Id
        //    });
        //    _dbContext.SaveChanges();



        //    return DiscountUseType.Success;
        //}

        public enum DiscountUseType
        {
            Success, ExpierDate, NotFound, Finished, UserUsed
        }
    }
}
