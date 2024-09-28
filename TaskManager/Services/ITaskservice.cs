using TaskManager.Dtos;

namespace TaskManager.Services
{
    public interface ITaskservice:ICommonService<TaskDto,InsertTaskDto, UpdateTaskDto,int>
    {

    }
}
