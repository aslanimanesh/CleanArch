using MyApp.Application.Interfaces;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;

namespace MyApp.Application.Services
{
    public class UsedUserDiscountService : GenericService<UsedUserDiscount>, IUsedUserDiscountService
    {
        private readonly IUsedUserDiscountRepository usedUserDiscountRepository;

        public UsedUserDiscountService(IUsedUserDiscountRepository usedUserDiscountRepository) : base(usedUserDiscountRepository) 
        {
            this.usedUserDiscountRepository = usedUserDiscountRepository;
        }
    }
}
