namespace InventoryApp.Core.Interfaces
{
    public interface IUnitOfWork 
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        Task<int> CompletedAsync();
    }
}
