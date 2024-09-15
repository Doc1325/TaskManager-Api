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
        ICommonService<UserDto,InsertUserDto,UpdateUserDto,string> _userService;
        public UserController([FromKeyedServices("UsersService")]ICommonService<UserDto,InsertUserDto,UpdateUserDto,string> userService) { 
          _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(InsertUserDto NewUser)
        {
            var userDto = await _userService.Add(NewUser);

            return Ok(userDto);

        }
        
    }
}
