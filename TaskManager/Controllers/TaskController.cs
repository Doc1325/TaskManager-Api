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
    /// <summary>
    /// Permite a los usuarios autorizados acceder y administra la información de las tareas dentro del sistema;
    /// </summary>
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

        /// <summary>
        /// Permite obtener un listado de las tareas a las cuales el usuario tiene acceso, filtrando solo las del estatus especificado.
        /// </summary>
        /// <returns>Devuelve todas las tareas del estatus especificado</returns>
        /// <remarks>
        /// Si el usuario es administrador, devolvera todas las tareas creadas por el mismo que corresponden al estatus especificado..
        /// Si el usuario es regular devolvera todas las tareas asignadas al mismo que corresponden al estatus especificado.
        /// </remarks>
        /// <response code="200">Devuelve todas las tareas a las cuales el usuario actual tiene permisos de consultar</response>
        
        [HttpGet()]
        [Authorize(Roles = "Admin, User")]
        public async Task<IEnumerable<TaskDto>> Get()
        {

            var TaskList = await _taskService.Get();
            return TaskList;
        }

        /// <summary>
        /// Permite obtener un listado de las tareas a las cuales el usuario tiene acceso, filtrando solo las del estatus especificado.
        /// </summary>
        /// <param name="statusid"></param>
        /// <returns>Devuelve todas las tareas del estatus especificado</returns>
        /// <remarks>
        /// Si el usuario es administrador, devolvera todas las tareas creadas por el mismo que corresponden al estatus especificado..
        /// Si el usuario es regular devolvera todas las tareas asignadas al mismo que corresponden al estatus especificado.
        /// </remarks>
        /// <response code="200">Devuelve todas las tareas a las cuales el usuario actual tiene permisos de consultar</response>
        [HttpGet("status={statusid}")]
        [Authorize(Roles = "Admin, User")]
        public IEnumerable<TaskDto> GetByStatus(int statusid)
        {
            var TaskList = _taskService.GetByFilter(statusid);
            return TaskList;


        }

        /// <summary>
        /// Permite crear una nueva tarea.
        /// </summary>
        /// <param name="NewTask"></param>
        /// <returns>Toda la informacion de la tarea agregada</returns>
        /// <remarks>\
        /// Los usuarios Administradores podran asignar tareas para si mismos y para otros usuarios
        /// Los usuarios Regulares podran asignar tareas unicamente para si mismos.
        /// Por ejemplo:
        ///
        ///     {
        ///        "id": 1,
        ///        "name": "Nuevo nombre",
        ///        "description": "Mi nueva descripcion",
        ///        "StatusId:" 2,
        ///        "assignedId": 1,
        ///        "CreatorId": 1,          
        ///     }
        /// </remarks>
        /// <response code="200">Devuelve la tarea agregada</response>
        /// <response code="400">La informacion de la tarea no esta completa, o es incorrecta</response>
        [HttpPost()]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Add(InsertTaskDto NewTask)
        {
            var validate = _taskValidator.Validate(NewTask);
            if (!validate.IsValid) return BadRequest(validate.Errors);
            var task = await _taskService.Add(NewTask);
            if (task == null) return BadRequest(_taskService.Errors);
            return CreatedAtAction(nameof(Get), task);



        }

        /// <summary>
        /// Actualiza una tarea ya existente.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="UpdatedTask"></param>
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
        ///     }
        /// </remarks>
        /// <response code="200">Devuelve la tarea eliminada</response>
        /// <response code="400">El id proporcionado no corresponde a una tarea existente, o la informacion a actualizar es invalida</response>

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, User")]

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
        /// Debe ser proporcionado el id de la tarea a eliminar
        /// </remarks>
        /// <response code="200">Devuelve la tarea eliminada</response>
        /// <response code="400">El id proporcionado no corresponde a una tarea existente o el usuario actual no es su creador</response>


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, User")]

        public async Task<IActionResult> Delete(int id) {


            var task = await _taskService.Delete(id);
            if(task == null) return BadRequest(_taskService?.Errors);

            return Ok(task);
        }
    }


}
