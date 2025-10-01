using TaskManager.Models.Enums;
namespace TaskManager.DTOs
{
    public class UpdateTaskDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Priority Priority { get; set; }
        public int? UserId { get; set; }
    }
}

