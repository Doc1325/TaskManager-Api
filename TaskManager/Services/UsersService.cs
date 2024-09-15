using AutoMapper;
using TaskManager.Dtos;
using TaskManager.Models;
using TaskManager.Repository;
using TaskManager.Utils;

namespace TaskManager.Services
{
    public class UsersService:ICommonService<UserDto,InsertUserDto,UpdateUserDto,string>
    {

        private IRepository<Users> _repository;
        private IMapper _mapper;

        public UsersService([FromKeyedServices("Users")] IRepository<Users> repository, IMapper mapper) { 
            _repository = repository;
            _mapper = mapper;

        }

        public List<string> Errors => throw new NotImplementedException();

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

        public Task<UserDto> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserDto>> Get()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserDto> GetByFilter(string filter)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> Update(UpdateUserDto updatedItem, int id)
        {
            throw new NotImplementedException();
        }
    }
}
