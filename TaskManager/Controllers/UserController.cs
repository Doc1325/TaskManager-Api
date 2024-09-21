using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public UserController( IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(InsertUserDto NewUser)
        {
            var userDto = await _userService.Add(NewUser);

            return Ok("Usuario: " + userDto.Username + " creado satisfactoriamente");

        }

        [HttpPost("Login")]
        public async Task<IActionResult> LogIn(InsertUserDto UserToValidate)
        {
            var UserToLog = _userService.IsValidUser(UserToValidate);

            if(UserToLog != null)
            {
                List<Claim> ClaimList = new List<Claim>();
                ClaimList.Add(new Claim(ClaimTypes.Name, UserToLog.Username));

                var Roles = UserToLog.RoleName.ToString().Split(",");

                foreach (var r in Roles)
                {
                    ClaimList.Add(new Claim(ClaimTypes.Role, r));
                }

                ClaimList.Add(new Claim(ClaimTypes.NameIdentifier, UserToLog.Id.ToString()));
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
    }

    }
