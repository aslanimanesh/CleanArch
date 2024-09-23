using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Interfaces;

namespace MyApp.Mvc.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult Index()
        {
            var products = _productService.GetProducts();
            return View(products);
        }
    }
}
