using MyApp.Application.Interfaces;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;

namespace MyApp.Application.Services
{
    public class UsedProductDiscountService : GenericService<UsedProductDiscount> , IUsedProductDiscountService
    {
        private readonly IUsedProductDiscountRepository _usedProductDiscountRepository;

        public UsedProductDiscountService(IUsedProductDiscountRepository usedProductDiscountRepository) : base(usedProductDiscountRepository)
        {
            _usedProductDiscountRepository = usedProductDiscountRepository;
        }
    }
}
