using InventoryApp.Service.DTO;

namespace InventoryApp.Service.Interface
{
    public interface IProductService
    {
        //Products: Get all, Get by ID, Create, Update, Delete.
        Task<IEnumerable<ProductDto>> GetAll(int? id);
        Task<ProductDto> GetById(int productId);
        Task<bool> AddAsync(ProductDto productDto);
        Task<bool> Update(int id, ProductDto productDto);
        Task<bool> Delete(int id);
        Task<int> CompletedAsync();
    }
}
