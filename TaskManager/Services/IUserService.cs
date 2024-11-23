using TaskManager.Dtos;
using TaskManager.Models;

namespace TaskManager.Services
{
    public interface IUserService: ICommonService<UserDto,InsertUserDto,UpdateUserDto,Func<Users,bool>>
    {
        public UserDto IsValidUser(InsertUserDto dto);
        public Task<UserDto> GetById(int id);
        public UserDto GetLoggedUser();

    }
}
