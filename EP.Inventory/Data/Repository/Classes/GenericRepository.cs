using EP.Inventory.Data.Persistence;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Services.Interfaces;
using System.Linq.Expressions;

namespace EP.Inventory.Data.Repository.Classes
{
    /// <summary>
    /// Provides a generic implementation of <see cref="IGenericRepository{TEntity}"/> 
    /// for performing CRUD operations. Works with any entity type and uses a shared database context.
    /// </summary>
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        // Data context
        private readonly DataContext _context;

        // Generic model
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(DataContext context)
        {
            _context = context;
            _dbSet   = _context.Set<TEntity>();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();

            if (include != null)
            {
                query = include(query).AsNoTracking();
            }

            return await query.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TEntity>> GetAllByConditionAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking().Where(predicate);

            if (include != null)
            {
                query = include(query).AsNoTracking();
            }

            return await query.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<TEntity?> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking().Where(predicate);

            if (include != null)
            {
                query = include(query).AsNoTracking();
            }

            return await query.FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TResult>> GetSelectedColumnsAsync<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            return await _dbSet.AsNoTracking().Select(selector).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        /// <inheritdoc/>
        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        /// <inheritdoc/>
        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        /// <inheritdoc/>
        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        /// <inheritdoc/>
        public async Task RemoveAsync(object Id)
        {
            // fetch the record from the table
            var entity = await _dbSet.FindAsync(Id);
            if (entity != null)
                _dbSet.Remove(entity);
        }

        /// <inheritdoc/>
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        /// <inheritdoc/>
        public async Task<bool> IsExist(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }
    }
}
