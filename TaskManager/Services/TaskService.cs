using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using TaskManager.Dtos;
using TaskManager.Models;
using TaskManager.Repository;

namespace TaskManager.Services
{
    public class TaskService : ICommonService<TaskDto, InsertTaskDto, UpdateTaskDto, int>
    {
       private IRepository<TaskItems> _repository;
        private IMapper _mapper;
        private ICommonService<StatusDto, InsertStatusDto, UpdateStatusDto, int> _statusService;
        public List<String> Errors { get; }
        public TaskService([FromKeyedServices("Tasks")]IRepository<TaskItems> repository,
            [FromKeyedServices("StatusService")] ICommonService<StatusDto, InsertStatusDto, UpdateStatusDto, int> StatusService, 
            IMapper mapper) {
            
            _repository = repository;
            _mapper = mapper;
            _statusService = StatusService;
            Errors = new List<String>();
            
        }
        public async Task<TaskDto> Add(InsertTaskDto NewTask)
        {

            if (_statusService.GetByFilter(NewTask.StatusId).IsNullOrEmpty())
            {

                Errors.Add("El Id del estatus es invalido");
                return null;

            }
            TaskItems item = _mapper.Map<TaskItems>(NewTask);
           
            await _repository.Add(item);
            await _repository.Save();
            TaskDto dto = _mapper.Map<TaskDto>(item);

            return dto;


        }

        public async Task<IEnumerable<TaskDto>> Get()
        {
           var TaskList = await _repository.Get();


            return TaskList.Select(t => _mapper.Map<TaskDto>(t));

        }

        public IEnumerable<TaskDto> GetByFilter(int StatusId)
        {
           var TaskList = _repository.GetByFilter(item => item.StatusId == StatusId);

            return TaskList.Select(t => _mapper.Map<TaskDto>(t));
        }

        public async Task<TaskDto> Delete(int id)
        {
          TaskItems TaskToRemove = await _repository.GetById(id);
            if (TaskToRemove == null)
            {
                Errors.Add("El Id de la tarea es invalido");
                return null;
            }
            TaskDto dto = _mapper.Map<TaskDto>(TaskToRemove);
            _repository.Delete(TaskToRemove);
            await _repository.Save();
            return dto;
        }

        public async Task<TaskDto> Update(UpdateTaskDto updatedItem, int id)
        {
            TaskItems TaskToUpdate = await _repository.GetById(id);
            if (TaskToUpdate == null)
            {
                Errors.Add("El Id de la tarea es invalido");
             
                return null;
            }
            if (_statusService.GetByFilter(updatedItem.StatusId).IsNullOrEmpty())
            {

                Errors.Add("El Id del estatus es invalido");

                return null;
            }
            TaskToUpdate = _mapper.Map<UpdateTaskDto, TaskItems>(updatedItem,TaskToUpdate);
            _repository.Update(TaskToUpdate);
            await _repository.Save();
            var dto = _mapper.Map<TaskDto>(TaskToUpdate);
            return dto;

        }
    }
}
