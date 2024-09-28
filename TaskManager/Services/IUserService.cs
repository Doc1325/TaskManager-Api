using TaskManager.Dtos;

namespace TaskManager.Services
{
    public interface IUserService: ICommonService<UserDto,InsertUserDto,UpdateUserDto,int>
    {
        public UserDto IsValidUser(InsertUserDto dto);
        public Task<UserDto> GetById(int id);
        public UserDto GetLoggedUser();

    }
}
