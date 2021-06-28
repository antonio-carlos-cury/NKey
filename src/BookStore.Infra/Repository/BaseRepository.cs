using BookStore.Domain.Entities;
using BookStore.Domain.Interfaces;
using BookStore.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infra.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {
        protected readonly BookStoreDbContext Db;
        protected readonly DbSet<TEntity> DbSet;

        protected Repository(BookStoreDbContext db)
        {
            Db = db;
            DbSet = db.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await DbSet.FindAsync(id);
        }
        public virtual async Task<int> CountAsync()
        {
            return await DbSet.CountAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            DbSet.Add(entity);
            await SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            DbSet.Update(entity);
            await SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            DbSet.Remove(new TEntity { Id = id });
            await SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Db?.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}
