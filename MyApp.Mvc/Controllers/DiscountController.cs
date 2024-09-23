using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Interfaces;

namespace MyApp.Mvc.Controllers
{
    public class DiscountController : Controller
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }
        // GET: DiscountController
        public ActionResult Index()
        {

            return View();
        }

        // GET: DiscountController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DiscountController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DiscountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DiscountController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DiscountController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DiscountController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DiscountController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
