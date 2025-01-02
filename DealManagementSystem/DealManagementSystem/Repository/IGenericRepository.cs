using DealManagementSystem.Models;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task SoftDeleteAsync(int id);
    Task DeleteAsync(T entity);
    IQueryable<T> Include(params Expression<Func<T, object>>[] includes);
    IIncludableQueryable<T, TProperty> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
   
}