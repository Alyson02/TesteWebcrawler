using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Domain.Entities;

namespace Core.Infrastructure.Repository
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task SaveChangesAsync();
        void SaveChanges();
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
        Task<int> ExecuteSQLAsync(string sql, params object[] parameteres);
        IRepository<T> Repository<T>() where T : class;

    }
}
