using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using TaskManager.Dtos;
using TaskManager.Models;
using TaskManager.Repository;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace TaskManager.Services
{
    public class TaskService : ITaskservice
    {
       private IRepository<TaskItems> _repository;
        private IMapper _mapper;
        private ICommonService<StatusDto, InsertStatusDto, UpdateStatusDto, int> _statusService;
        private IUserService _userService;
        public List<String> Errors { get; }
        private readonly IHttpContextAccessor _http;
        public TaskService([FromKeyedServices("Tasks")]IRepository<TaskItems> repository,
            [FromKeyedServices("StatusService")] ICommonService<StatusDto, InsertStatusDto, UpdateStatusDto, int> StatusService, 
            IMapper mapper
             ,IUserService userService,
            IHttpContextAccessor http) {
            
            _repository = repository;
            _mapper = mapper;
            _statusService = StatusService;
            Errors = new List<String>();
            _userService = userService;
            _http = http;  
            
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

        public async Task<IEnumerable<TaskDto>> Get(bool IsAdmin)
        {
            var userId = int.Parse(_http.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());
            Func<TaskItems, bool> filter;

            filter = t => t.AsignnedId == userId;

            if (IsAdmin) filter = t => t.CreatorId == userId;
            filter = t => t.AsignnedId == userId;

            var TaskList = _repository.GetByFilter(filter);

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

            UserDto userToVerify = await _userService.GetById(TaskToRemove.CreatorId);

            if (userToVerify.Username != _http.HttpContext.User.Identity.Name  )
            {
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
            TaskDto dto = new TaskDto();
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
            if(TaskToUpdate.CreatorId == int.Parse(_http.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString()))
            {

                TaskToUpdate = _mapper.Map<UpdateTaskDto, TaskItems>(updatedItem, TaskToUpdate);
                _repository.Update(TaskToUpdate);
                await _repository.Save();
                dto = _mapper.Map<TaskDto>(TaskToUpdate);
                return dto;

            } 
            
            if (TaskToUpdate.AsignnedId == int.Parse(_http.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString()))
            {
                if(TaskToUpdate.Title != updatedItem.Title || TaskToUpdate.Description != updatedItem.Description)
                {
                    Errors.Add("No tienes permisos para modificar el nombre ni descripcion de esta tarea");

                    return null;
                }

                TaskToUpdate = _mapper.Map<UpdateTaskDto, TaskItems>(updatedItem, TaskToUpdate);
                _repository.Update(TaskToUpdate);
                await _repository.Save();
                dto = _mapper.Map<TaskDto>(TaskToUpdate);
                return dto;


            }


            return null;
        }
    }
}
