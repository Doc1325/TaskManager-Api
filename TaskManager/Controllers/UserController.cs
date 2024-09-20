using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Dtos;
using TaskManager.Services;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService _userService;
        public UserController( IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(InsertUserDto NewUser)
        {
            var userDto = await _userService.AddUser(NewUser);

            return Ok("Usuario: " + userDto.Username + " creado satisfactoriamente");

        }

        [HttpPost("Login")]
        public async Task<IActionResult> LogIn(InsertUserDto UserToValidate)
        {
            var user = _userService.IsValidUser(UserToValidate);

            if(user != null)
            {
                return Ok("El usuario existe");
            }
            return NotFound("El usuario no existe");




        }
    }

    }
