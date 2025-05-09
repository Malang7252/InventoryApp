using FluentValidation;
using InventoryApp.Core.Interfaces;
using InventoryApp.Infrastructure.UOW;
using InventoryApp.Service.DTO;
using InventoryApp.Service.Interface;
using InventoryApp.Service.Service;
using InventoryApp.Service.Validators;

namespace InventoryApp.API.Extensions
{
    public static class CustomServiceExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICategoryService, CategoryService>();
        }
        public static void AddValidator(this IServiceCollection services)
        {
            services.AddScoped<IValidator<ProductDto>, ProductDtoValidator>();
            services.AddScoped<IValidator<CategoryDto>, CategoryDtoValidator>();
        }
    }
}
