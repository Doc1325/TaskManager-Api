using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using TaskManager.Dtos;
using TaskManager.Models;
using TaskManager.Repository;
using TaskManager.Utils;
using static System.Net.WebRequestMethods;


namespace TaskManager.Services
{
    public class UsersService: IUserService
    {
         

        private IRepository<Users> _repository;
        private IMapper _mapper;
        private readonly IHttpContextAccessor _http;


        public List<String> Errors { get; }

        public UsersService([FromKeyedServices("Users")] IRepository<Users> repository, IMapper mapper, IHttpContextAccessor http) { 
            _repository = repository;
            _mapper = mapper;
            _http = http;
            Errors = new List<String>();

        }


        public async Task<UserDto> Add(InsertUserDto Insertitem)
        {

            var ExistUser = _repository.GetByFilter(u => String.Equals(u.Username, Insertitem.Username, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            if (ExistUser != null)
            {
                Errors.Add("Ya existe un usuario con este nombre");
                return null;
            }
            Insertitem.Password = PassEncrypter.EncryptPassword(Insertitem.Password);
            var NewUser = _mapper.Map<Users>(Insertitem);

            NewUser.RoleName = "User"; 

            await _repository.Add(NewUser);
            await _repository.Save();
            UserDto User = _mapper.Map<UserDto>(NewUser);

            return User;
           
        }

        public UserDto IsValidUser(InsertUserDto user)
        {
            var UserToLog =  _mapper.Map<UserDto>(_repository.GetByFilter((u) => u.Username == user.Username &&
            u.Password == PassEncrypter.EncryptPassword(user.Password)).FirstOrDefault() );

            if(UserToLog != null)
            {
              
                
                return UserToLog;

            }

            
            return null;
        }

        public async Task<UserDto> GetById(int id)
        {
            Users user = await _repository.GetById(id);
           var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task<UserDto> Delete(int id)
        {
            var userToDelete = await _repository.GetById(id);
            if(userToDelete == null)
            {
                Errors.Add("Usuario invalido o no existente");
                return null;
            }
            if (userToDelete.RoleName == "Admin") {
                Errors.Add(
                    "No se pueden eliminar los usuarios de administrador por esta vía" + 
                    "Contacte al departamento de soporte para asistencia");
                return null;
            }

            UserDto DeletedUser = _mapper.Map<UserDto>(userToDelete);
            _repository.Delete(userToDelete);
            await _repository.Save();
            return DeletedUser;
        }

        public async Task<UserDto> Update(UpdateUserDto updatedItem, int id)
        {
            updatedItem.Id = id;
            var UserToUpdate = await _repository.GetById(id);
            if(UserToUpdate == null)
            {
                Errors.Add("Usuario invalido");
                return null;
            }

            if(UserToUpdate.Username != updatedItem.UserName)
            {
                Errors.Add("No se puede modificar el nombre de usuario");
                return null;
            }
            UserToUpdate = _mapper.Map<UpdateUserDto, Users>(updatedItem,UserToUpdate);
             _repository.Update(UserToUpdate);
            await _repository.Save();
            var dto = _mapper.Map<UserDto>(UserToUpdate);
            return dto;

        }

        public async Task<IEnumerable<UserDto>> Get()
        {
            var UserList = await _repository.Get();

            return UserList.Select(u => _mapper.Map<UserDto>(u));
        }

        public IEnumerable<UserDto> GetByFilter(Func<Users,bool> filter)
        {
          var UsersByRole =  _repository.GetByFilter(filter);
          return UsersByRole.Select(u => _mapper.Map<UserDto>(u));

        }

        public UserDto GetLoggedUser()
        {

            ClaimsPrincipal? user = _http?.HttpContext?.User;
            if (user == null) return null;
            
            return   new UserDto
            {
                Id = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value.ToString()),
                RoleName = user.FindFirst(ClaimTypes.Role)?.Value.ToString()
            };

        }
    }
}
