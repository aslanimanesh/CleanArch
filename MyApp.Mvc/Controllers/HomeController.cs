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
            var products = await _productService.GetAllAsync();

            var productViewModels = products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Price = p.Price,
                ImageName = p.ImageName,
            }).ToList();

            return View(productViewModels);
        }
    }
}
