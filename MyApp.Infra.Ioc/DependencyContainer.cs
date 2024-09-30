using Microsoft.Extensions.DependencyInjection;
using MyApp.Application.Interfaces;
using MyApp.Application.Services;
using MyApp.Domain.Interfaces;
using MyApp.Infa.Data.Repositories;

namespace MyApp.Infra.Ioc
{
    public static class DependencyContainer
    {
        public static void RegisterService(IServiceCollection services)
        {

            #region Application Layer
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<IDiscountAssignmentToProductService, DiscountAssignmentToProductService>();
            services.AddScoped<IDiscountAssignmentToUserService, DiscountAssignmentToUserService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderDetailsService, OrderDetailsService>();
            services.AddScoped<IPaymentService , PaymentService>();
            services.AddScoped<IUsedUserDiscountService, UsedUserDiscountService>();
            services.AddScoped<IUsedProductDiscountService , UsedProductDiscountService>();

            #endregion

            #region Infra Data Layer
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            services.AddScoped<IProductDiscountRepository, ProductDiscountRepository>();
            services.AddScoped<IUserDiscountRepository, UserDiscountRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderDetailsRepository, OrderDetailsRepository>();
            services.AddScoped<IUsedUserDiscountRepository, UsedUserDiscountRepository>();
            services.AddScoped<IUsedProductDiscountRepository, UsedProductDiscountRepository>();
            #endregion

        }
    }
}
