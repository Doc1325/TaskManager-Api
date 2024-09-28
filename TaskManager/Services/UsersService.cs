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


        public List<string> Errors => throw new NotImplementedException();

        public UsersService([FromKeyedServices("Users")] IRepository<Users> repository, IMapper mapper, IHttpContextAccessor http) { 
            _repository = repository;
            _mapper = mapper;
            _http = http;

        }


        public async Task<UserDto> Add(InsertUserDto Insertitem)
        {
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

        public Task<UserDto> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> Update(UpdateUserDto updatedItem, int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserDto>> Get()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserDto> GetByFilter(int filter)
        {
            throw new NotImplementedException();
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
