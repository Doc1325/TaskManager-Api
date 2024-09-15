using AutoMapper;
using TaskManager.Dtos;
using TaskManager.Models;

namespace TaskManager.Mappers
{
    public class UserMapper: Profile
    {
        public UserMapper() {

            CreateMap<Users, UserDto>();
            CreateMap<InsertUserDto, Users>();
            CreateMap<UpdateUserDto, Users>();



        }
    }
}
