using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Dtos;
using TaskManager.Models;
using TaskManager.Repository;
using TaskManager.Services;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {

       private ITaskservice _taskService;
       private IValidator<InsertTaskDto> _taskValidator;
        private IValidator<UpdateTaskDto> _updateTaskValidator;


        public TaskController([FromKeyedServices("TaskService")]ITaskservice TaskService,
            [FromKeyedServices("TasksValidator")] IValidator<InsertTaskDto> TaskValidator,
            [FromKeyedServices("UpdateTasksValidator")] IValidator<UpdateTaskDto> UpdateTaskValidator)
        { 
               _taskService = TaskService;
            _taskValidator = TaskValidator;
            _updateTaskValidator = UpdateTaskValidator;

        }

        [HttpGet("CreatedTasks")]
        [Authorize(Roles = "Admin")]

        public async Task <IEnumerable<TaskDto>> GetCreated()
        {

            var TaskList = await _taskService.Get(true);
            return TaskList;


        }




        [HttpGet("AssignedTasks")]
        [Authorize(Roles = "User")]

        public async Task<IEnumerable<TaskDto>> GetAssigned()
        {

            var TaskList = await _taskService.Get(false);
            return TaskList;


        }




        [HttpGet("{statusid}")]
       public  IEnumerable<TaskDto> GetByStatus(int statusid)
        {
            var TaskList = _taskService.GetByFilter(statusid);
            return TaskList;


        }



        
        [HttpPost()]
        [Authorize(Roles = "Admin, User")]

        public async Task<IActionResult> Add(InsertTaskDto NewTask)
        {
            var validate = _taskValidator.Validate(NewTask);
            if (!validate.IsValid) return BadRequest(validate.Errors);
            var task = await _taskService.Add(NewTask);
            if (task == null) return BadRequest(_taskService.Errors);
            return Ok(task);



        }


        [HttpPut("{id}")]
       public async Task<IActionResult>  Update(int id, UpdateTaskDto UpdatedTask)
        {
           var validate = _updateTaskValidator.Validate(UpdatedTask);
            if (!validate.IsValid) return BadRequest(validate.Errors);
          var task = await _taskService.Update(UpdatedTask, id);
           if(task == null) return BadRequest(_taskService?.Errors);
            return Ok(task);

        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]

        public async Task<IActionResult> Delete(int id) {


            var task = await _taskService.Delete(id);
                       if(task == null) return BadRequest(_taskService?.Errors);

            return Ok(task);
        }
    }


}
