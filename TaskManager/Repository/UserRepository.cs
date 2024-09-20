using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Repository
{
    public class UserRepository : IRepository<Users>
    {
        private TaskContext _taskContext;

        public UserRepository(TaskContext taskContext)
        {
            _taskContext = taskContext;
        }
        public async Task Add(Users entity)
        {
           await _taskContext.AddAsync(entity);
        }

    public void Delete(Users entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Users>> Get()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Users> GetByFilter(Func<Users, bool> filter)
        {
            return _taskContext.Users.Where(filter);

        }

        public Task<Users> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Save()
        {
            await _taskContext.SaveChangesAsync();

        }

        public void Update(Users entity)
        {
            throw new NotImplementedException();
        }
    }
}
