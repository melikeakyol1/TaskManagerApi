using TaskManager.Models.Enums;
namespace TaskManager.DTOs
{
    public class UserDto
    {
        public int Id { get; init; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<TaskDto> Tasks { get; set; } = new();
    }
}

