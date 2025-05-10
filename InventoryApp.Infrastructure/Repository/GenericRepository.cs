using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using InventoryApp.Core.Interfaces;
using InventoryApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InventoryApp.Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected AppDbContext _context;
        protected DbSet<T> dbSet;
        protected readonly ILogger _logger;
        public GenericRepository(AppDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
            dbSet = _context.Set<T>();
        }

        public async Task<bool> Add(T entity)
        {
            await dbSet.AddAsync(entity);
            return true;
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return dbSet.Where(expression);
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? predicate = null)
        {
            if (predicate != null)
            {
                return await dbSet.Where(predicate).ToListAsync();
            }

            return await dbSet.ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAll(int? id)
        {
            return await dbSet.ToListAsync();
        }

        public async Task<T?> GetById(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<bool> Remove(int id)
        {
            var t = await dbSet.FindAsync(id);

            if (t != null)
            {
                dbSet.Remove(t);
                return true;
            }
            else
                return false;
        }

        public async Task<bool> Update(int id, T entity)
        {
            //dbSet.Update(entity);
            //return true;
            var existing = await dbSet.FindAsync(id);

            if (existing == null)
                throw new KeyNotFoundException($"Entity with id {id} not found.");

            _context.Entry(existing).CurrentValues.SetValues(entity);

            try
            {
                //await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Optional: log and rethrow or return false
                throw new Exception("Concurrency conflict occurred while updating.", ex);
            }
        }
    }
}
