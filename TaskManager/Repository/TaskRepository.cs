using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManager.Models;

namespace TaskManager.Repository
{
    public class TaskRepository : IRepository<TaskItems>
    {

        private TaskContext _context;

        public TaskRepository(TaskContext context)
        {
            _context = context;
        }
        public async Task Add(TaskItems entity)
        {
         await _context.Tasks.AddAsync(entity);
        }

        public void Delete(TaskItems entity)
        {
            _context.Tasks.Remove(entity);

        }

        public async Task<IEnumerable<TaskItems>> Get()
        {
            return await _context.Tasks.ToListAsync();
        }
        public async Task<TaskItems> GetById(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }
        public IEnumerable<TaskItems> GetByFilter (Func<TaskItems, bool> filter)
        {
         return  _context.Tasks.Where(filter).ToList();


        }

        public void Update(TaskItems entity)
        {
            _context.Tasks.Attach(entity);
            _context.Tasks.Entry(entity).State = EntityState.Modified;

        }


        public async Task Save()
        {
        await  _context.SaveChangesAsync();
        }
    }
}
