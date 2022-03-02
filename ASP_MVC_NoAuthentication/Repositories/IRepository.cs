namespace ASP_MVC_NoAuthentication.Repositories
{
    //Repository project pattern
    public interface IRepository<TEntity, TId>
    {
        TEntity GetById(TId id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        List<TEntity> GetAll();
    }
}
