using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using TaskManager.Dtos;
using TaskManager.Models;
using TaskManager.Repository;
using TaskManager.Utils;

namespace TaskManager.Services
{
    public class UsersService: IUserService
    {

        private IRepository<Users> _repository;
        private IMapper _mapper;

        public UsersService([FromKeyedServices("Users")] IRepository<Users> repository, IMapper mapper) { 
            _repository = repository;
            _mapper = mapper;

        }


        public async Task<UserDto> AddUser(InsertUserDto Insertitem)
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

    
    }
}
