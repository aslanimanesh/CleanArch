using MyApp.Application.Interfaces;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;

namespace MyApp.Application.Services
{
    public class DiscoutService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscoutService(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        } 


        public IEnumerable<Discount> GetDiscounts()
        {
            return _discountRepository.GetDiscounts();
        }

        public Discount GetDiscountById(int DiscountId)
        {
            return _discountRepository.GetDiscountById(DiscountId);
        }

        public bool CreateDiscount(Discount discount)
        {
            return _discountRepository.CreateDiscount(discount);    
        }

        public bool UpdateDiscount(Discount discount)
        {
            return _discountRepository.UpdateDiscount(discount);
        }

        public bool DeleteDiscount(int discountId)
        {
            return _discountRepository.DeleteDiscount(discountId);
        }
    }
}
