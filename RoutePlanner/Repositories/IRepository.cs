namespace RoutePlanner.Repositories
{
    //Repository project pattern
    public interface IRepository<TEntity, TId>
    {
        Task<TEntity> GetByIdAsync(TId id);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task RemoveAsync(TEntity entity);
        Task<List<TEntity>> GetAllAsync();
    }
}
