using DealManagementSystem.Data;
using DealManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DealManagementSystem.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _entities;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        // Get an entity by its ID (including soft-deleted entities if necessary)
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _entities.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        }

        // Get all entities, excluding soft-deleted ones
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _entities.Where(e => !e.IsDeleted).ToListAsync();
        }

        // Add a new entity to the database
        public async Task<T> AddAsync(T entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = null; // New entities don't have an updated timestamp initially
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // Update an existing entity
        public async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            entity.UpdatedAt = DateTime.UtcNow; // Set the update timestamp
            await _context.SaveChangesAsync();
        }

        // Soft delete an entity (mark IsDeleted as true)
        public async Task SoftDeleteAsync(int id)
        {
            var entity = await _entities.FindAsync(id);
            if (entity == null) return;

            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow; // Update the timestamp
            await _context.SaveChangesAsync();
        }

        // Delete an entity permanently from the database (hard delete)
        public async Task DeleteAsync(T entity)
        {
            // Ensure the entity is being tracked by the context
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _entities.Attach(entity);  // Attach the entity if it's not being tracked
            }

            _entities.Remove(entity);  // Remove the itinerary entity from the DbSet
            await _context.SaveChangesAsync();  // Persist the changes to the database
        }

        // Get the first entity that matches the specified condition
        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entities.FirstOrDefaultAsync(predicate);
        }

        public IQueryable<T> Include(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _entities;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query;
        }

        public IIncludableQueryable<T, TProperty> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath)
        {
            return _entities.Include(navigationPropertyPath);
        }

       
      
    }
}