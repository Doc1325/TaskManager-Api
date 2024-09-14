using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TaskManager.Dtos;
using TaskManager.Services;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private ICommonService<StatusDto, InsertStatusDto, UpdateStatusDto, int> _statusService;
        private IValidator<InsertStatusDto> _statusValidator;
        private IValidator<UpdateStatusDto> _updateStatusValidator;


        public StatusController([FromKeyedServices("StatusService")] ICommonService<StatusDto, InsertStatusDto, UpdateStatusDto, int> StatusService,
            [FromKeyedServices("StatusValidator")] IValidator<InsertStatusDto> validator, 
            [FromKeyedServices("UpdateStatusValidator")] IValidator<UpdateStatusDto> UpdateStatusValidator)
        {
            _statusService = StatusService;
            _statusValidator = validator;
            _updateStatusValidator = UpdateStatusValidator;
        }

        [HttpGet]
        public Task<IEnumerable<StatusDto>> Get()
        {
            var list = _statusService.Get();
            return list;
        }

        [HttpPost]
        public async Task<IActionResult> Add(InsertStatusDto NewStatus)
        {

            var validator = _statusValidator.Validate(NewStatus);

            if (!validator.IsValid) return BadRequest(validator.Errors);
            var status = await _statusService.Add(NewStatus);
            if (status == null) return BadRequest(_statusService.Errors);
            
            return Ok(status);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Update(int Id, UpdateStatusDto dto)
        {

            var validation = _updateStatusValidator.Validate(dto);
            if(!validation.IsValid) return BadRequest(validation.Errors);
            var status = await _statusService.Update(dto, Id);
            if (status == null) return BadRequest(_statusService.Errors);
            return Ok(status);


        }


        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {

            var status = await _statusService.Delete(Id);
            if (status == null) return BadRequest(_statusService.Errors);
            return Ok(status);
        }
       

    }

}
