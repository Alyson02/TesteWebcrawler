//using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Context;
using Core.Domain.Entities;

namespace Core.Infrastructure.Repository
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
    {
        private readonly TContext _context;
        private Hashtable _repositories;
        private IDbContextTransaction _transaction;
        public UnitOfWork(TContext dbContext)
        {
            this._context = dbContext;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }
      
        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            if(_transaction != null)
                await _transaction.CommitAsync(cancellationToken);
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
                await _transaction.RollbackAsync(cancellationToken);
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);

                var repositoryInstance =
                    Activator.CreateInstance(repositoryType
                        .MakeGenericType(typeof(TEntity)), _context);

                _repositories.Add(type, repositoryInstance);
            }

            return (IRepository<TEntity>)_repositories[type];
        }

        public async Task<int> ExecuteSQLAsync(string sql, params object[] parameteres)
        {
            return await _context.Database.ExecuteSqlRawAsync(sql, parameteres);
        }
    }
}
