using InventoryApp.Service.DTO;

namespace InventoryApp.Service.Interface
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAll(int? id);
        Task<CategoryDto> GetById(int productId);
        Task<bool> AddAsync(CategoryDto categoryDto);
        Task<bool> Update(int id, CategoryDto categoryDto);
        Task<bool> Delete(int id);
        Task<int> CompletedAsync();
    }
}
