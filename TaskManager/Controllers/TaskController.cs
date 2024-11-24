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

        [HttpGet()]
        [Authorize(Roles = "Admin, User")]
        public async Task<IEnumerable<TaskDto>> Get()
        {

            var TaskList = await _taskService.Get();
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
            return CreatedAtAction(nameof(Get), new {task.Id}, task);



        }

        /// <summary>
        /// Actualiza una tarea ya existente.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="UpdatedItem"></param>

        /// <returns>La informacion actualizada de la tarea</returns>
        /// <remarks>
        /// La tareas pueden ser actualizadas por un usuario administrador, por el usuario asignado, o por el usuario creador de la tarea.
        /// Por ejemplo:
        ///
        ///     {
        ///        "id": 1,
        ///        "name": "Nuevo nombre",
        ///        "description": "Mi nueva descripcion",
        ///        "StatusId:" 2,
        ///        "assignedId": 1,
        ///        "CreatorId": 1,
        ///          
        ///     }
        /// </remarks>
        /// <response code="200">Devuelve la tarea eliminada</response>
        /// <response code="400">El id proporcionado no corresponde a una tarea existente, o la informacion a actualizar es invalida</response>

        [Authorize(Roles = "Admin, User")]
        [HttpPut("{id}")]
       public async Task<IActionResult>  Update(int id, UpdateTaskDto UpdatedTask)
        {
           var validate = _updateTaskValidator.Validate(UpdatedTask);
            if (!validate.IsValid) return BadRequest(validate.Errors);
          var task = await _taskService.Update(UpdatedTask, id);
           if(task == null) return BadRequest(_taskService?.Errors);
            return Ok(task);

        }


        /// <summary>
        /// Elimina una tarea creada por el usuario actual.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>La tarea eliminada</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "Tarea eliminada",
        ///        "description": "Mi descripcion",
        ///        "StatusId:" 2,
        ///        "assignedId": 1,
        ///        "CreatorId": 1,
        ///          
        ///     }
        /// </remarks>
        /// <response code="200">Devuelve la tarea eliminada</response>
        /// <response code="400">El id proporcionado es no corresponde a una tarea existente</response>


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, User")]

        public async Task<IActionResult> Delete(int id) {


            var task = await _taskService.Delete(id);
            if(task == null) return BadRequest(_taskService?.Errors);

            return Ok(task);
        }
    }


}
