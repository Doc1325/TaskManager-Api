using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using TaskManager.Dtos;
using TaskManager.Services;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(InsertUserDto NewUser)
        {
            var userDto = await _userService.Add(NewUser);
            if (userDto == null) return BadRequest(_userService.Errors);
            return Ok("Usuario: " + userDto.Username + " creado satisfactoriamente");

        }

        [HttpPost("Login")]
        public async Task<IActionResult> LogIn(InsertUserDto UserToValidate)
        {
            var UserToLog = _userService.IsValidUser(UserToValidate);

            if (UserToLog != null)
            {
                List<Claim> ClaimList = new List<Claim>();
                ClaimList.Add(new Claim(ClaimTypes.Name, UserToLog.Username));

                var Roles = UserToLog.RoleName.ToString().Split(",");

                foreach (var r in Roles)
                {
                    ClaimList.Add(new Claim(ClaimTypes.Role, r));
                }

                ClaimList.Add(new Claim(ClaimTypes.NameIdentifier, UserToLog.id.ToString()));
                AuthenticationProperties props = new AuthenticationProperties()
                {
                    AllowRefresh = true
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(ClaimList,
                CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    props
                    );
                return Ok("Autenticacion exitosa");
            }
            return Unauthorized("Usuario o contraseña incorrectos");



        }

        [HttpPost("Logout")]
        [Authorize("Admin, User")]

        public async Task<IActionResult> Logout() {
            try
            {
                await HttpContext.SignOutAsync();
                return Ok("Sesion cerrada con exito");

            }
            catch (Exception)
            {

                return BadRequest("Error al cerrar la sesion");
            }
       
        }

        [HttpGet("UsersList")]
        [Authorize("Admin, User")]
        public async Task<IActionResult> GetUsers()
        {
            var Users = await _userService.Get();
            if (Users == null) return BadRequest(_userService.Errors);
            return Ok(Users);

        }


        [HttpDelete("{UserId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveUsers(int UserId)
        {
            var DeletedUser = await _userService.Delete(UserId);
            if (DeletedUser == null) return BadRequest(_userService.Errors);
            return Ok(DeletedUser);

        }

        [HttpPut("{UserId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(int UserId,UpdateUserDto updateUser)
        {
            var updatedUser = await _userService.Update(updateUser,UserId);
            if (updatedUser == null) return BadRequest(_userService.Errors);
            return Ok(updatedUser);

        }


    }

    }
