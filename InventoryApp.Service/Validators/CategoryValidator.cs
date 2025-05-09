using FluentValidation;
using InventoryApp.Service.DTO;
using InventoryApp.Service.Interface;

namespace InventoryApp.Service.Validators
{
    public class CategoryDtoValidator : AbstractValidator<CategoryDto>
    {
        private readonly ICategoryService _categoryService;
        public CategoryDtoValidator(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("CategoryId must be provided.")
                //.WhenAsync(CategoryExistsAsync)
                .WhenAsync((dto, cancellation) => CategoryExistsAsync(dto.Id, cancellation))
                .WithMessage("CategoryId does not exist.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Category name cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Category Description is required.");
        }
        private async Task<bool> CategoryExistsAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _categoryService.GetById(id);

            return result != null;
        }
    }
}
