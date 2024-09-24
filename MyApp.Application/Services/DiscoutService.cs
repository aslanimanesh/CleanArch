using MyApp.Application.Interfaces;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;

namespace MyApp.Application.Services
{
    public class DiscoutService : GenericService<Discount>, IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscoutService(IDiscountRepository discountRepository) : base(discountRepository)
        {
            _discountRepository = discountRepository;
        }
    }
}
