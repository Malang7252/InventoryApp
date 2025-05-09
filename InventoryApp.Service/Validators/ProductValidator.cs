using FluentValidation;
using InventoryApp.Service.DTO;
using InventoryApp.Service.Interface;

namespace InventoryApp.Service.Validators
{
    public class ProductDtoValidator : AbstractValidator<ProductDto>
    {
        private readonly ICategoryService _categoryService;
        public ProductDtoValidator(ICategoryService categoryService)
        {
            _categoryService = categoryService;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.") // Price must be positive.
                .PrecisionScale(8, 2, true).WithMessage("Price must have at most 8 digits in total and 2 decimals.");
            
            RuleFor(p => p.Quantity).GreaterThanOrEqualTo(0).WithMessage("Quantity must be greater than 0.");
            
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId must be provided.")
                .WhenAsync((dto, cancellation) => CategoryExistsAsync(dto.CategoryId, cancellation))
                .WithMessage("CategoryId does not exist.");
        }
        private async Task<bool> CategoryExistsAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _categoryService.GetById(id);
            
            return result == null;
        }
    }
}
