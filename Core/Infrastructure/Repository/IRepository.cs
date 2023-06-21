using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Core.Domain.Models;

namespace Core.Infrastructure.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<T> FindAsync(ISpecification<T> spec, CancellationToken cancellationToken = default, bool asNoTracking = true);
        Task<IEnumerable<T>> ListAsync(CancellationToken cancellationToken = default, bool asNoTracking = true);
        Task<IEnumerable<T>> ListAsync(ISpecification<T> spec, CancellationToken cancellationToken = default, bool asNoTracking = true);
        Task InsertAsync(T entity, CancellationToken cancellationToken = default);
        Task InsertRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(IEnumerable<T> entities);
        Task<bool> ExistsAsync(ISpecification<T> spec, CancellationToken cancellation = default);
        IEnumerable<TResult> Select<TResult>(ISpecification<T> specification, Func<T, TResult> selector);
    }
}
