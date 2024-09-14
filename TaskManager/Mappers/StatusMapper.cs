using AutoMapper;
using TaskManager.Dtos;
using TaskManager.Models;

namespace TaskManager.Mappers
{
    public class StatusMapper:Profile
    {
        public StatusMapper() {

            CreateMap<Status, StatusDto>();
            CreateMap<InsertStatusDto,Status>();
            CreateMap<UpdateStatusDto, Status>();

        }
    }
}
