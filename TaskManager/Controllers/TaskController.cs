﻿using FluentValidation;
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

       private ICommonService<TaskDto, InsertTaskDto, UpdateTaskDto,int> _taskService;
       private IValidator<InsertTaskDto> _taskValidator;
        private IValidator<UpdateTaskDto> _updateTaskValidator;


        public TaskController([FromKeyedServices("TaskService")]ICommonService<TaskDto,InsertTaskDto,UpdateTaskDto,int> TaskService,
            [FromKeyedServices("TasksValidator")] IValidator<InsertTaskDto> TaskValidator,
            [FromKeyedServices("UpdateTasksValidator")] IValidator<UpdateTaskDto> UpdateTaskValidator)
        { 
               _taskService = TaskService;
            _taskValidator = TaskValidator;
            _updateTaskValidator = UpdateTaskValidator;

        }

        [HttpGet]
        public async Task <IEnumerable<TaskDto>> Get()
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
       public  async Task<IActionResult> Delete(int id) {


            var task = await _taskService.Delete(id);
                       if(task == null) return BadRequest(_taskService?.Errors);

            return Ok(task);
        }
    }


}