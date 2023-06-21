using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Entities;

namespace Core.Context
{
    public class TesteElawDbContext : DbContext
    {
        public TesteElawDbContext(DbContextOptions<TesteElawDbContext> options) : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<ExecutionEntity> Execution { get; set; }
    }
}
