using System.Linq.Expressions;

namespace SharedLibrary.Services.Interfaces
{
    /// <summary>
    /// Defines the basic CRUD operations contracts for entities of type <typeparamref name="TEntity"/>.
    /// This interface abstracts the interaction with the database context for the specified entity type.
    /// </summary>
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Retrieves all entities of type <typeparamref name="TEntity"/> with optional eager loading of related data.
        /// </summary>
        /// <param name="include">A function to include related entities using Include/ThenInclude.</param>
        /// <returns>
        /// A collection of entities of type <typeparamref name="TEntity"/>.
        /// </returns>
        Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);

        /// <summary>
        /// Retrieves the list entity that matches the specified condition.
        /// </summary>
        /// <param name="predicate">The filter expression to apply to the query.</param>
        /// <param name="include">A function to include related entities using Include/ThenInclude.</param>
        /// <returns>
        /// A collection of entities of type <typeparamref name="TEntity"/>.
        /// </returns>
        Task<IEnumerable<TEntity>> GetAllByConditionAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);

        /// <summary>
        /// Retrieves the first entity that matches the specified condition.
        /// </summary>
        /// <param name="predicate">The filter expression to apply to the query.</param>
        /// <param name="include">A function to include related entities using Include/ThenInclude.</param>
        /// <returns>
        /// The type of <typeparamref name="TEntity"/> entity if found; otherwise null.
        /// </returns>
        Task<TEntity?> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);

        /// <summary>
        /// Retrieves a list of projected results by selecting specific columns from the entity.
        /// </summary>
        /// <param name="selector">An expression that defines which fields to select from the entity.</param>
        /// <returns>
        /// A collection of entities of type <typeparamref name="TResult"/>.
        /// </returns>
        Task<IEnumerable<TResult>> GetSelectedColumnsAsync<TResult>(Expression<Func<TEntity, TResult>> selector);

        /// <summary>
        /// Checks whether the entity exists in the database.
        /// </summary>
        /// <param name="predicate">The filter expression to apply to the query.</param>
        /// <returns>
        /// <c>true</c> if the entity exists; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> IsExist(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Adds a new entity of type <typeparamref name="TEntity"/> to the database.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        Task AddAsync(TEntity entity);

        /// <summary>
        /// Adds a list of entities of type <typeparamref name="TEntity"/> to the database.
        /// </summary>
        /// <param name="entities">The entity list to be added.</param>
        Task AddRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Updates an existing entity of type <typeparamref name="TEntity"/> in the database.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        void Update(TEntity entity);

        /// <summary>
        /// Removes an entity of type <typeparamref name="TEntity"/> from the database.
        /// </summary>
        /// <param name="entity">The entity to be deleted.</param>
        void Remove(TEntity entity);

        /// <summary>
        /// Removes an entity from the database.
        /// </summary>
        /// <param name="Id">The identifier (PK) of the entity to be deleted.</param>
        Task RemoveAsync(object Id);

        /// <summary>
        /// Saves all changes made in the repository to the database.
        /// </summary>
        /// <returns>
        /// A boolean value indicating whether the changes were saved successfully.
        /// </returns>
        Task<bool> SaveChangesAsync();
    }
}
