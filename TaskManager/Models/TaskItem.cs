using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Models.Enums;  

namespace TaskManager.Models
{
    public class TaskItem
    {
        [Key] // Birincil anahtar
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required,MaxLength(100)]
        public string Title { get; set; }

        [Required, MaxLength(2000)]
        public string Description { get; set; }

        public bool IsCompleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public Priority Priority { get; set; } = Priority.medium;

        // Foreign Key
        
        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
