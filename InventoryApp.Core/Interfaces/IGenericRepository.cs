using System.Linq.Expressions;

namespace InventoryApp.Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(int? id);
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? predicate = null);
        Task<T?> GetById(int id);
        Task<bool> Add(T entity);
        Task<bool> Remove(int id);
        Task<bool> Update(int id, T entity);
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
    }
}
