namespace TaskManager.Dtos
{
    public class UpdateTaskDto
    {

            public int id {  get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int StatusId { get; set; }
        
    }
}
