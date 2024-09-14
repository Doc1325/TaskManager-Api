namespace TaskManager.Services
{
    public interface ICommonService<T,TI,TU,TF>
    {
        public Task<T> Add(TI Insertitem);
        public Task<T> Delete(int id);

        public Task<T> Update (TU updatedItem, int id);   
        public Task<IEnumerable<T>> Get();
        public IEnumerable<T> GetByFilter(TF filter);

        public List<String> Errors { get; }

    }

}
