using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Context;
using Core.Domain.Entities;
using Core.Domain.Models;

namespace Core.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _dbContext;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<T> FindAsync(ISpecification<T> spec, CancellationToken cancellationToken = default, bool asNoTracking = true)
        {
            var query = _dbContext.Set<T>().AsQueryable();
            query = spec.Includes
                .Aggregate(query, (current, include) => current.Include(include));

            query = spec.IncludeStrings
                .Aggregate(query, (current, include) => current.Include(include));

            query = query.Where(spec.Criteria);

            // Apply ordering if expressions are set
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.GroupBy != null)
            {
                query = query.GroupBy(spec.GroupBy).SelectMany(x => x);
            }

            if (asNoTracking)
                return await query.AsNoTracking().FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

            return await query.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<IEnumerable<T>> ListAsync(CancellationToken cancellationToken = default, bool asNoTracking = true)
        {
            if (asNoTracking)
                return await _dbContext.Set<T>().AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);

            return await _dbContext.Set<T>().ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<IEnumerable<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default, bool asNoTracking = true)
        {

            var query = _dbContext.Set<T>().AsQueryable();

            // modify the IQueryable using the specification's criteria expression
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            // Includes all expression-based includes
            query = specification.Includes.Aggregate(query,
                                    (current, include) => current.Include(include));

            // Include any string-based include statements
            query = specification.IncludeStrings.Aggregate(query,
                                    (current, include) => current.Include(include));

            // Apply ordering if expressions are set
            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            if (specification.GroupBy != null)
            {
                query = query.GroupBy(specification.GroupBy).SelectMany(x => x);
            }

            // Apply paging if enabled
            if (specification.IsPagingEnabled)
            {
                query = query.Skip(specification.Skip)
                             .Take(specification.Take);
            }

            if (asNoTracking)
                return await query.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);

            return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual IEnumerable<TResult> Select<TResult>(ISpecification<T> specification, Func<T, TResult> selector)
        {

            var query = _dbContext.Set<T>().AsQueryable();

            // Includes all expression-based includes
            query = specification.Includes.Aggregate(query,
                                    (current, include) => current.Include(include));

            // Include any string-based include statements
            query = specification.IncludeStrings.Aggregate(query,
                                    (current, include) => current.Include(include));

            // modify the IQueryable using the specification's criteria expression
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            // Apply ordering if expressions are set
            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }

            else if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            // Apply paging if enabled
            if (specification.IsPagingEnabled)
            {
                query = query.Skip(specification.Skip)
                             .Take(specification.Take);
            }

            return query.Select(selector);
        }

        public virtual async Task InsertAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbContext.ChangeTracker.Clear();
            await _dbContext.Set<T>().AddAsync(entity, cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task InsertRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            _dbContext.ChangeTracker.Clear();
            await _dbContext.Set<T>().AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
        }

        public virtual Task UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            return Task.CompletedTask;
        }

        public virtual Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().UpdateRange(entities);
            return Task.CompletedTask;
        }

        public virtual Task DeleteAsync(T entity)
        {
            _dbContext.ChangeTracker.Clear();
            _dbContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public virtual Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(ISpecification<T> spec, CancellationToken cancellation = default)
        {
            var query = _dbContext.Set<T>().AsQueryable();
            // Includes all expression-based includes
            query = spec.Includes.Aggregate(query,
                                    (current, include) => current.Include(include));

            // Include any string-based include statements
            query = spec.IncludeStrings.Aggregate(query,
                                    (current, include) => current.Include(include));

            return query.AnyAsync(spec.Criteria, cancellation);
        }
    }
}
