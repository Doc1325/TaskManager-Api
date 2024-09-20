using TaskManager.Dtos;

namespace TaskManager.Services
{
    public interface IUserService
    {
        public Task<UserDto> AddUser(InsertUserDto dto);
        public UserDto IsValidUser(InsertUserDto dto);

    }
}
