using Microsoft.AspNetCore.Mvc;

namespace MyApp.Mvc.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
