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
          _taskContext.Users.Remove(entity);
        }

        public async Task<IEnumerable<Users>> Get()
        {
           var List = await _taskContext.Users.ToListAsync();
           return List;
        }

        public IEnumerable<Users> GetByFilter(Func<Users, bool> filter)
        {
            return _taskContext.Users.Where(filter);

        }

        public async Task<Users> GetById(int id)
        {
            return await _taskContext.Users.FindAsync(id);
        }

        public async Task Save()
        {
            await _taskContext.SaveChangesAsync();

        }

        public void Update(Users entity)
        {
           _taskContext.Users.Attach(entity);
           _taskContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
