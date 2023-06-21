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
using Core.Infrastructure.Repository;

namespace Core.Business.Services.Base
{
    public abstract class ServiceBaseAbstract<T, TContext> where T : class where TContext : DbContext
    {
        private readonly IUnitOfWork<TContext> _unitOfWork;
        private readonly IRepository<T> _repository;

        public ServiceBaseAbstract(IUnitOfWork<TContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = unitOfWork.Repository<T>();
        }

        public Specification<T> CreateSpec(Expression<Func<T, bool>> criteria)
        {
            return new Specification<T>(criteria);
        }

        public Specification<T> CreateSpec()
        {
            return new Specification<T>();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            await _repository.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            await _repository.DeleteRangeAsync(entities);
            await _unitOfWork.SaveChangesAsync();
        }

        public virtual async Task<bool> ExistsAsync(ISpecification<T> spec, CancellationToken cancellation = default)
        {
            return await _repository.ExistsAsync(spec, cancellation);
        }

        public virtual async Task<T> FindAsync(ISpecification<T> spec, CancellationToken cancellationToken = default, bool asNoTracking = true)
        {
            return await _repository.FindAsync(spec, cancellationToken, asNoTracking);
        }

        public virtual async Task InsertAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _repository.InsertAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        public virtual async Task InsertRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _repository.InsertRangeAsync(entities, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        public virtual async Task<IEnumerable<T>> ListAsync(CancellationToken cancellationToken = default, bool asNoTracking = true)
        {
            return await _repository.ListAsync(cancellationToken, asNoTracking);
        }

        public virtual async Task<IEnumerable<T>> ListAsync(ISpecification<T> spec, CancellationToken cancellationToken = default, bool asNoTracking = true)
        {
            return await _repository.ListAsync(spec, cancellationToken, asNoTracking);
        }

        public IEnumerable<TResult> Select<TResult>(ISpecification<T> specification, Func<T, TResult> selector)
        {
            return _repository.Select(specification, selector);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            await _repository.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            await _repository.UpdateRangeAsync(entities);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
