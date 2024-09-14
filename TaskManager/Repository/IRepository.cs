namespace TaskManager.Repository
{
    public interface IRepository <TEntity>
    {

         Task<IEnumerable<TEntity>> Get();
        Task<TEntity> GetById(int id);

        IEnumerable<TEntity> GetByFilter(Func<TEntity, bool> filter);
        Task Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

        Task Save();


    }
}
