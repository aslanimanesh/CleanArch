using Microsoft.AspNetCore.Mvc;

namespace MyApp.Mvc.Areas.Admin.Controllers
{
    [Area(areaName: "Admin")]
    public class AdminBaseController : Controller
    {
        protected string SuccessMessage = "SuccessMessage";
        protected string ErrorMessage = "ErrorMessage";
    }
}
