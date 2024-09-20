﻿using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Repository
{
    public class StatusRepository : IRepository<Status>
    {
        private TaskContext _taskContext;

        public StatusRepository(TaskContext taskContext)
        {
            _taskContext = taskContext;
        }
        public async Task Add(Status entity)
        {
         await _taskContext.Status.AddAsync(entity);
        }

        public void Delete(Status entity)
        {
             _taskContext.Status.Remove(entity);
        }

        public async Task<IEnumerable<Status>> Get()
        {
           var StatusList =  await _taskContext.Status.ToListAsync();
            return StatusList;
        }

        public IEnumerable<Status> GetByFilter(Func<Status, bool> filter)
        {
            return  _taskContext.Status.Where(filter).ToList();
        }

        public async Task<Status> GetById(int id)
        {
            return await _taskContext.Status.FindAsync(id);
        }

        public async Task Save()
        {
          await _taskContext.SaveChangesAsync();
        }

        public void Update(Status entity)
        {
            _taskContext.Attach(entity);
            _taskContext.Status.Entry(entity).State = EntityState.Modified;
        }
    }
}