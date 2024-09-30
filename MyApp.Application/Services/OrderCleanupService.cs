using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyApp.Domain.Interfaces;

// این کلاس برای پاک‌سازی سفارش‌های قدیمی طراحی شده است و از BackgroundService ارث می‌برد
public class OrderCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider; // سرویس ارائه‌دهنده برای دریافت سایر سرویس‌ها
    private readonly ILogger<OrderCleanupService> _logger; // لاگر برای ثبت لاگ‌ها

    // سازنده که IServiceProvider و ILogger را به کلاس تزریق می‌کند
    public OrderCleanupService(IServiceProvider serviceProvider, ILogger<OrderCleanupService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    // متد ExecuteAsync که در آن منطق اصلی سرویس پیاده‌سازی می‌شود
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested) // تا زمانی که درخواست لغو نشده باشد
        {
            using (var scope = _serviceProvider.CreateScope()) // ایجاد یک محدوده برای استفاده از Scoped Services
            {
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>(); // دریافت ریپوزیتوری سفارش
                await CleanupOrdersAsync(orderRepository); // فراخوانی متد برای پاک‌سازی سفارش‌ها
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // انتظار یک ساعت قبل از اجرای مجدد
        }
    }

    // متد CleanupOrdersAsync برای بررسی و حذف سفارش‌های قدیمی
    private async Task CleanupOrdersAsync(IOrderRepository orderRepository)
    {
        // دریافت تمامی سفارش‌ها از ریپوزیتوری
        var ordersToDelete = await orderRepository.GetAllAsync();

        // بررسی هر سفارش
        foreach (var order in ordersToDelete)
        {
            // اگر سفارش هنوز نهایی نشده باشد و بیش از 48 ساعت از زمان ایجاد آن گذشته باشد
            if (!order.IsFinaly && (DateTime.Now - order.CreateDate).TotalMinutes > 1)
            {
                // ثبت لاگ برای حذف سفارش
                _logger.LogInformation($"Deleting order with ID: {order.Id} - Status: {order.IsFinaly}");

                // حذف سفارش از دیتابیس با استفاده از شیء Order
                await orderRepository.DeleteAsync(order); // تغییر به حذف بر اساس شیء به‌جای ID
            }
        }
    }

}
