using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Interfaces;
using MyApp.Domain.Interfaces;
using MyApp.Domain.ViewModels.Products;
using System.Security.Claims;

namespace MyApp.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductDiscountRepository _productDiscountRepository;
        private readonly IUserDiscountRepository _userDiscountRepository;

        public HomeController(IProductService productService , IProductDiscountRepository productDiscountRepository,
            IUserDiscountRepository userDiscountRepository)
        {
            _productService = productService;
            _productDiscountRepository = productDiscountRepository;
            _userDiscountRepository = userDiscountRepository;
        }

        public async Task<IActionResult> Index()
        {
            string userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int? userId = null;
            if (int.TryParse(userIdStr, out int parsedUserId))
            {
                userId = parsedUserId;
            }

            var products = await _productService.GetAllAsync();

            var productViewModels = new List<ProductViewModelWithCalculateDiscount>();

            foreach (var product in products)
            {
                decimal finalPrice = product.Price;
                decimal discountPercentage = 0;

                var productDiscounts = await _productDiscountRepository.GetDiscountsForProductAsync(product.Id);

                if (userId.HasValue)
                {
                    var userDiscounts = await _userDiscountRepository.GetDiscountsForUserAsync(userId.Value);
                    foreach (var productDiscount in productDiscounts)
                    {
                        if (userDiscounts.Any(ud => ud.DiscountId == productDiscount.DiscountId))
                        {
                            discountPercentage = productDiscount.Discount.DiscountPercentage;
                            finalPrice *= (1 - (productDiscount.Discount.DiscountPercentage / 100));
                        }
                    }
                }
                else
                {
                    foreach (var productDiscount in productDiscounts)
                    {
                        discountPercentage = productDiscount.Discount.DiscountPercentage;
                        finalPrice *= (1 - (productDiscount.Discount.DiscountPercentage / 100));
                    }
                }

                productViewModels.Add(new ProductViewModelWithCalculateDiscount
                {                   
                    Product = product,
                    FinalPrice = finalPrice,
                    DiscountPercentage = discountPercentage,
                    

                });
            }

            return View(productViewModels);
        }

    }
}
