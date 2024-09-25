using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Application.Interfaces;
using MyApp.Domain.Models;
using MyApp.Domain.ViewModels.Order;
using System.Security.Claims;

namespace MyApp.Mvc.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IOrderDetailsService _orderDetailsService;
        private readonly IProductService _productService;

        public OrderController(IOrderService orderService, IOrderDetailsService orderDetailsService,
            IProductService productService)
        {
            _orderService = orderService;
            _orderDetailsService = orderDetailsService;
            _productService = productService;
        }
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
                    Sum = 0
                };
                
                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                await _orderService.AddAsync(order);
                await _orderDetailsService.AddAsync(new OrderDetail()
                {
                    OrderId = order.OrderId,
                    Count = 1,
                    Price = product.Price,
                    ProductId = id
                });
         
            }
            else
            {
                var details = await _orderDetailsService.ExistProductInOrderDetail(order.OrderId, id);
                if (details == null)
                {
                   
                    var product = await _productService.GetByIdAsync(id);
                    if (product == null)
                    {
                        return NotFound(); 
                    }

                    await _orderDetailsService.AddAsync(new OrderDetail()
                    {
                        OrderId = order.OrderId,
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
            //UpdateSumOrder(order.OrderId);
            return RedirectToAction("ShowOrder", "Order");

        }

        public async Task<IActionResult> ShowOrder()
        {
            int CurrentUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            Order order = await _orderService.HasPendingOrder(CurrentUserID);

            List<ShowOrderViewModel> _list = new List<ShowOrderViewModel>();
            if (order != null)
            {
                var details = await _orderDetailsService.GetAllOrderDetailByOrderId(order.OrderId);
                foreach (var item in details)
                {
                    var product = await _productService.GetByIdAsync(item.ProductId);

                    _list.Add(new ShowOrderViewModel()
                    {
                        Count = item.Count,
                        ImageName = product.ImageName,
                        OrderDetailId = item.OrderDetailID,
                        Price = item.Price,
                        Sum = item.Count * item.Price,
                        Title = product.Title
                    });

                }
            }

            return View(_list);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _orderDetailsService.DeleteAsync(id);

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
                            await _orderDetailsService.DeleteAsync(orderDetail.OrderDetailID);
                        }
                        else
                        {
                            await _orderDetailsService.UpdateAsync(orderDetail);
                        }

                        break;
                    }
            }

            return RedirectToAction("ShowOrder", "Order");
        }
        public async void UpdateSumOrder(int orderId)
        {
            var order = await _orderService.GetByIdAsync(orderId);
            //order.Sum = _ctx.OrderDetails.Where(o => o.OrderId == order.OrderId).Select(d => d.Count * d.Price).Sum();
            //Sum = _ctx.OrderDetails.Where(o => o.OrderId == order.OrderId).Select(d => d.Count * d.Price).Sum();
            await _orderService.UpdateAsync(order);
            
        }
    }
}

