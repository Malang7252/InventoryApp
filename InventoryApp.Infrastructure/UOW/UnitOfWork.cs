using InventoryApp.Core.Interfaces;
using InventoryApp.Infrastructure.Data;
using InventoryApp.Infrastructure.Repository;
using Microsoft.Extensions.Logging;

namespace InventoryApp.Infrastructure.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;
        public IProductRepository Products { get; private set; }
        public ICategoryRepository Categories { get; private set; }

        public UnitOfWork(AppDbContext context, ILoggerFactory logger)
        {
            _context = context;
            _logger = logger.CreateLogger("logs");
            Products = new ProductRepository(_context, _logger);
            Categories = new CategoryRepository(_context, _logger);
        }

        public async Task<int> CompletedAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
