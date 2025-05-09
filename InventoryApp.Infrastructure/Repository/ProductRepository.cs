using InventoryApp.Core.Entities;
using InventoryApp.Core.Interfaces;
using InventoryApp.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace InventoryApp.Infrastructure.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
