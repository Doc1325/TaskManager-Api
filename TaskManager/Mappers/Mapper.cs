using AutoMapper;
using TaskManager.Dtos;
using TaskManager.Models;

namespace TaskManager.Mappers
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<TaskItems, TaskDto>();
            CreateMap<InsertTaskDto, TaskItems>();
            CreateMap<UpdateTaskDto,TaskItems >().ForMember(b => b.Id, t => t.MapFrom(a => a.id));



        }
    }
}
