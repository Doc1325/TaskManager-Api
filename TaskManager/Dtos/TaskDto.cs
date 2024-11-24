﻿namespace TaskManager.Dtos
{
    public class TaskDto
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
        public int CreatorId { get; set; }
        public int AssignedId { get; set; }

    }
}
