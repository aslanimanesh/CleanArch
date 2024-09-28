using MyApp.Application.Interfaces;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;

namespace MyApp.Application.Services
{
    public class UsedProductDiscountService : GenericService<UsedProductDiscount> , IUsedProductDiscountService
    {
        public UsedProductDiscountService(IUsedProductDiscountRepository usedProductDiscountRepository) : base(usedProductDiscountRepository)
        {
            
        }
    }
}
