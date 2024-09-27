using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Application.Interfaces;
using MyApp.Domain.Models;
using MyApp.Domain.ViewModels.Order;
using MyApp.Infa.Data.Context;
using System.Security.Claims;
using ZarinpalSandbox;

namespace MyApp.Mvc.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IOrderDetailsService _orderDetailsService;
        private readonly IProductService _productService;
        private readonly IDiscountService _discountService;
        private readonly MyAppDbContext _dbContext;

        public OrderController(IOrderService orderService, IOrderDetailsService orderDetailsService,
            IProductService productService , IDiscountService discountService ,MyAppDbContext  dbContext)
        {
            _orderService = orderService;
            _orderDetailsService = orderDetailsService;
            _productService = productService;
            _discountService = discountService;
            _dbContext = dbContext;
            
        }
        [Authorize]
        public async Task<IActionResult> AddToCart(int id)
        {
            int CurrentUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            Order order = await _orderService.HasPendingOrder(CurrentUserID);
            if (order == null)
            {
                order = new Order()
                {
                    UserId = CurrentUserID,
                    CreateDate = DateTime.Now,
                    IsFinaly = false,
                    Sum = 0,
                };
                
                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                await _orderService.AddAsync(order);
                await _orderDetailsService.AddAsync(new OrderDetail()
                {
                    OrderId = order.Id,
                    Count = 1,
                    Price = product.Price,
                    ProductId = id
                });

                await UpdateSumOrder(order.Id);

            }
            else
            {
                var details = await _orderDetailsService.ExistProductInOrderDetail(order.Id, id);
                if (details == null)
                {
                   
                    var product = await _productService.GetByIdAsync(id);
                    if (product == null)
                    {
                        return NotFound(); 
                    }

                    await _orderDetailsService.AddAsync(new OrderDetail()
                    {
                        OrderId = order.Id,
                        Count = 1,
                        Price = product.Price,
                        ProductId = id
                    });
                }
                else
                {
                    details.Count += 1;
                    await _orderDetailsService.UpdateAsync(details);
                }

            }

            await UpdateSumOrder(order.Id);

            return RedirectToAction("ShowOrder", "Order");

        }

        [Authorize]
        public async Task<IActionResult> ShowOrder()
        {
            int CurrentUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            Order order = await _orderService.HasPendingOrder(CurrentUserID);
            

            List<ShowOrderViewModel> _list = new List<ShowOrderViewModel>();
            if (order != null)
            {
                var details = await _orderDetailsService.GetAllOrderDetailByOrderId(order.Id);
                foreach (var item in details)
                {
                    var product = await _productService.GetByIdAsync(item.ProductId);

                    _list.Add(new ShowOrderViewModel()
                    {
                        Count = item.Count,
                        ImageName = product.ImageName,
                        OrderDetailId = item.Id,
                        Price = item.Price,
                        Sum = item.Count * item.Price,
                        Title = product.Title
                    });

                }
                ViewBag.OrderID = order.Id;
                ViewBag.Sum = order.Sum;
            }

            return View(_list);
        }

        public async Task<IActionResult> Delete(int id)
        {
           
            var orderDetail = await _orderDetailsService.GetByIdAsync(id);
            await _orderDetailsService.DeleteAsync(id);
            await UpdateSumOrder(orderDetail.OrderId);

            return RedirectToAction("ShowOrder", "Order");
        }

        public async Task<IActionResult> Command(int id, string command)
        {
            var orderDetail =await _orderDetailsService.GetByIdAsync(id);

            switch (command)
            {
                case "up":
                    {
                        orderDetail.Count += 1;
                        await _orderDetailsService.UpdateAsync(orderDetail);
                        break;
                    }
                case "down":
                    {
                        orderDetail.Count -= 1;
                        if (orderDetail.Count == 0)
                        {
                            await _orderDetailsService.DeleteAsync(orderDetail.Id);
                        }
                        else
                        {
                            await _orderDetailsService.UpdateAsync(orderDetail);
                        }

                        break;
                    }
            }

            await UpdateSumOrder(orderDetail.OrderId);

            return RedirectToAction("ShowOrder", "Order");
        }

        public async Task UpdateSumOrder(int orderId)
        {
            var order = await _orderService.GetByIdAsync(orderId);
            order.Sum = await _orderService.GetOrderTotalAsync(orderId);
            await _orderService.UpdateAsync(order);

        }
        [Authorize]
        [HttpPost]
        
        public async Task<IActionResult> UseDiscount(string discountCode, int orderId)
        {
            

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // اعمال تخفیف بر روی فاکتور
            var resultMessage = await _discountService.ApplyDiscountToOrderAsync(discountCode, orderId, userId);

            // نمایش نتیجه به کاربر
            TempData["DiscountResult"] = resultMessage;
            //return RedirectToAction("Index", new { orderId });
            return RedirectToAction("ShowOrder", "Order");
        }

        [Authorize]
        public IActionResult Payment()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var order = _dbContext.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.UserId == userId && !o.IsFinaly);
            if (order == null)
                return NotFound();

            var payment = new Payment((int)order.OrderDetails.Sum(d => d.Price));
            var res = payment.PaymentRequest($"پرداخت فاکتور شماره {order.Id}",
                "http://localhost:1635/Order/OnlinePayment/" + order.Id, "Iman@Madaeny.com", "09197070750");
            if (res.Result.Status == 100)
            {
                return Redirect("+" + res.Result.Authority);
            }
            else
            {
                return BadRequest();
            }

        }

        public IActionResult OnlinePayment(int id)
        {
            if (HttpContext.Request.Query["Status"] != "" &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" &&
                HttpContext.Request.Query["Authority"] != "")
            {
                string authority = HttpContext.Request.Query["Authority"].ToString();
                var order = _dbContext.Orders.Include(o => o.OrderDetails)
                    .FirstOrDefault(o => o.Id == id);
                var payment = new Payment((int)order.OrderDetails.Sum(d => d.Price));
                var res = payment.Verification(authority).Result;
                if (res.Status == 100)
                {
                    order.IsFinaly = true;
                    _dbContext.Orders.Update(order);
                    _dbContext.SaveChanges();
                    ViewBag.code = res.RefId;
                    return View();
                }
            }

            return NotFound();
        }

    }
}

