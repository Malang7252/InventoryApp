using FluentValidation;
using FluentValidation.Results;
using InventoryApp.Core.Interfaces;
using InventoryApp.Infrastructure.UOW;
using InventoryApp.Service.DTO;
using InventoryApp.Service.Interface;
using InventoryApp.Service.Service;
using InventoryApp.Service.Validators;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InventoryApp.API.Extensions
{
    public static class CustomServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICategoryService, CategoryService>();
            return services;
        }
        public static IServiceCollection AddValidator(this IServiceCollection services)
        {
            services.AddScoped<IValidator<ProductDto>, ProductDtoValidator>();
            services.AddScoped<IValidator<CategoryDto>, CategoryDtoValidator>();
            return services;
        }
        public static void AddToModelState(this ValidationResult result, ModelStateDictionary modelState)
        {
            foreach (var error in result.Errors)
            {
                modelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }

    }
}
