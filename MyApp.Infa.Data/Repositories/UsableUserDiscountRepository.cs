using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using MyApp.Infa.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Infa.Data.Repositories
{
    public class UsableUserDiscountRepository : GenericRepository<UsableUserDiscount>, IUsableUserDiscount
    {
        private readonly MyAppDbContext _dbContext;

        public UsableUserDiscountRepository(MyAppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UsableUserDiscount> GetUsableUserDiscountAsync(int userId, int discountId)
        {
           return await _dbContext.UsableUserDiscounts
          .SingleOrDefaultAsync(ud => ud.UserId == userId && ud.DiscountId == discountId);
        }
    }
}
