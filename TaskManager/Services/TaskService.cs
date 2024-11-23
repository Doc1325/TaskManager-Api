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
        public TaskService([FromKeyedServices("Tasks")]IRepository<TaskItems> repository,
            [FromKeyedServices("StatusService")] ICommonService<StatusDto, InsertStatusDto, UpdateStatusDto, int> StatusService, 
            IMapper mapper
             ,IUserService userService) {
            
            _repository = repository;
            _mapper = mapper;
            _statusService = StatusService;
            _userService = userService;
            Errors = new List<String>();

        }
 
        public async Task<TaskDto> Add(InsertTaskDto NewTask)
        {
            UserDto userLogged = _userService.GetLoggedUser();



            if (NewTask.CreatorId != userLogged.Id)
            {
                Errors.Add("No puedes crear una tarea en nombre de otro usuario");
                return null;
            }

            if (NewTask.AsignnedId != userLogged.Id && userLogged.RoleName != "Admin")
            {
                Errors.Add("No tienes permisos para asignar tareas a otro usuario");
                return null;
            }

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

            UserDto userLogged = _userService.GetLoggedUser();

            Func<TaskItems, bool> filter;


            if (userLogged.RoleName == "Admin") filter = t => t.CreatorId == userLogged.Id 
            || t.AssignedId == userLogged.Id;
            else filter = t => t.AssignedId == userLogged.Id ;

            var TaskList =  _repository.GetByFilter(filter);

            return TaskList.Select(t => _mapper.Map<TaskDto>(t));

        }

        public IEnumerable<TaskDto> GetByFilter(int StatusId)

        {
            UserDto userLogged = _userService.GetLoggedUser();


            Func<TaskItems, bool> filter;

            if (userLogged.RoleName == "Admin") filter = t => t.CreatorId == userLogged.Id && t.StatusId == StatusId;
            else filter = t => t.AssignedId == userLogged.Id && t.StatusId == StatusId;

            var TaskList = _repository.GetByFilter(filter);

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

            UserDto userCreator = await _userService.GetById(TaskToRemove.CreatorId);
            UserDto userLogged = _userService.GetLoggedUser();

            if (userCreator.Id != userLogged.Id  )
            {
                Errors.Add("No eres el creador de esta tarea, por tanto no puedes eliminarla");
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
            UserDto userLogged = _userService.GetLoggedUser();
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
            if (TaskToUpdate.CreatorId == userLogged.Id || userLogged.RoleName == "Admin")
            //solo el usuario creador o un admin pueden actualizar todos los parametros
            {

                TaskToUpdate = _mapper.Map<UpdateTaskDto, TaskItems>(updatedItem, TaskToUpdate);
                _repository.Update(TaskToUpdate);
                await _repository.Save();
                dto = _mapper.Map<TaskDto>(TaskToUpdate);
                return dto;

            }

            if (TaskToUpdate.AssignedId != userLogged.Id)

            if (TaskToUpdate.AssignedId == userLogged.Id)
            {
                return null;


            }
            if (TaskToUpdate.Title != updatedItem.Title || TaskToUpdate.Description != updatedItem.Description || TaskToUpdate.AssignedId != updatedItem.AsignnedId)
            {
                Errors.Add("Solo tienes permiso para modificar el estatus de esta tarea");
                return null;
            }

            TaskToUpdate = _mapper.Map<UpdateTaskDto, TaskItems>(updatedItem, TaskToUpdate);
            _repository.Update(TaskToUpdate);
            await _repository.Save();
            dto = _mapper.Map<TaskDto>(TaskToUpdate);
            return dto;
        }
    }
}
