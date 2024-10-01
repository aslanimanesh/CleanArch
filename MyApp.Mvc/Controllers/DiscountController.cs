using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Interfaces;

namespace MyApp.Mvc.Controllers
{
    public class DiscountController : Controller
    {

        #region Fields
        private readonly IDiscountService _discountService;
        #endregion

        #region Constructor
        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }
        #endregion

        #region Use Discount By Code

        [Authorize] // Requires user to be authenticated
        [HttpPost]
        public async Task<IActionResult> UseDiscount(string discountCode, int orderId , int userId)
        {

            // Apply the discount to the order
            var resultMessage = await _discountService.ApplyDiscountToOrderAsync(discountCode, orderId, userId);

            // Show the result message to the user
            TempData["DiscountResult"] = resultMessage;

            if (resultMessage == "Success")
            {
                TempData["IsDiscountApplied"] = true;
                TempData["DiscountResult"] = "تخفیف با موفقیت اعمال شد";           
            }    

            return RedirectToAction("ShowOrder", "Order"); // Redirect to show the order
        }

        #endregion

    }
}
