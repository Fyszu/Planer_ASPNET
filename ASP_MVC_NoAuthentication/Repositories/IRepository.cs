namespace ASP_MVC_NoAuthentication.Repositories
{
    //Repository project pattern
    public interface IRepository<TEntity, TId>
    {
        Task<TEntity> GetById(TId id);
        Task Add(TEntity entity);
        Task Update(TEntity entity);
        Task Remove(TEntity entity);
        Task<List<TEntity>> GetAll();
    }
}
