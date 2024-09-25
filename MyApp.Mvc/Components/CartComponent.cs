using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MyApp.Mvc.Components
{
    //public class CartComponent:ViewComponent
    //{
        //private MyContext _ctx;

        //public CartComponent(MyContext ctx)
        //{
        //    _ctx = ctx;
        //}

        //public async Task<IViewComponentResult> InvokeAsync()
        //{
        //    List<ShowCartViewModel> _list=new List<ShowCartViewModel>();

        //    if (User.Identity.IsAuthenticated)
        //    {
        //        string CurrentUserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        //        var order = _ctx.Orders.SingleOrDefault(o => o.UserId == CurrentUserID && !o.IsFinaly);
        //        if (order != null)
        //        {
        //            var details = _ctx.OrderDetails.Where(d => d.OrderId == order.OrderId).ToList();
        //            foreach (var item in details)
        //            {
        //                var product = _ctx.Products.Find(item.ProductId);
        //                _list.Add(new ShowCartViewModel()
        //                {
        //                    Count = item.Count,
        //                    Title = product.Title,
        //                    ImageName = product.ImageName
        //                });

        //            }
        //        }

        //    }

        //    return View("/Views/Shared/_ShowCart.cshtml", _list);
        //}
    //}
}
