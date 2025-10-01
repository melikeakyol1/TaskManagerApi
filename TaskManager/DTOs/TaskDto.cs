using TaskManager.Models.Enums;

/*namespace TaskManager.DTOs
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsCompleted { get; set; }
        public int Priority { get; set; }     

        // User bilgisinden sadece ihtiyacın olanı
        public string? UserName { get; set; }
    }
}*/
namespace TaskManager.DTOs
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public Priority Priority { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }


    }
    public class TaskCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? UserId { get; set; }
        public Priority Priority { get; set; }
        public bool IsCompleted { get; set; }
    }
}


