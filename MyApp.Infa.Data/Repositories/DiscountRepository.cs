using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using MyApp.Infa.Data.Context;

namespace MyApp.Infa.Data.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly MyAppDbContext _dbContext;

        public DiscountRepository(MyAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool CreateDiscount(Discount discount)
        {
            _dbContext.Discounts.Add(discount);
            return Save();
            
        }

        public bool DeleteDiscount(int discountId)
        {
            var discount = _dbContext.Discounts.Find(discountId);
            if (discount != null)
            {
                _dbContext.Discounts.Remove(discount);
                return Save();
            }
            return false;
        }

        public Discount GetDiscountById(int DiscountId)
        {
            return _dbContext.Discounts.Find(DiscountId);
        }

        public IEnumerable<Discount> GetDiscounts()
        {
            return _dbContext.Discounts;
        }

        public bool UpdateDiscount(Discount discount)
        {
            _dbContext.Discounts.Update(discount);
            return Save();
        }
        private bool Save()
        {
            return _dbContext.SaveChanges() > 0;
        }
    }
}
