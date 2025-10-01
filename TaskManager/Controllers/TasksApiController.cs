using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.DTOs;
using Microsoft.AspNetCore.SignalR;
using System.Net.Security;

namespace TaskManager.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TasksApiController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        public TasksApiController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var allTasks = await dbContext.Tasks
                .Include(t => t.User)
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    UserId = t.UserId,
                    UserName = t.User != null ? t.User.UserName : null,
                    Priority = t.Priority,
                    IsCompleted = t.IsCompleted,
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();

            return Ok(allTasks);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTaskById([FromRoute] int id)
        {
            var task = await dbContext.Tasks
                .Include(t => t.User)
                .Where(t => t.Id == id)
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    UserId = t.UserId,
                    UserName = t.User != null ? t.User.UserName : null,
                    Priority = t.Priority,
                    IsCompleted = t.IsCompleted,
                    CreatedAt = t.CreatedAt
                })
                .SingleOrDefaultAsync();

            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpGet("byUser/{userId:int}")]
        public async Task<IActionResult> GetTasksByUser(int userId)
        {
            var tasks = await dbContext.Tasks
                .Include(t => t.User)
                .Where(t => t.UserId == userId) // t.UserId daha iyi performans
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    UserId = t.UserId,
                    UserName = t.User != null ? t.User.UserName : null,
                    Priority = t.Priority,
                    IsCompleted = t.IsCompleted,
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();

            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(TaskCreateDto request)
        {
            // Kullanıcı var mı kontrol et
            var user = await dbContext.Users.FindAsync(request.UserId);
            if (user == null)
                return BadRequest("UserId veritabanında yok.");

            var task = new TaskItem
            {
                Title = request.Title,
                Description = request.Description,
                IsCompleted = request.IsCompleted,
                UserId = request.UserId,
                Priority = request.Priority,
                CreatedAt = DateTime.UtcNow
            };

            dbContext.Tasks.Add(task);
            await dbContext.SaveChangesAsync();

            // Response olarak TaskDto döndür
            var taskDto = new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                UserId = task.UserId,
                UserName = user.UserName,
                Priority = task.Priority,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt
            };

            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, taskDto);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTask(int id, UpdateTaskDto updateTaskDto)
        {
            var task = await dbContext.Tasks.FindAsync(id);
            if (task == null) return NotFound();

            task.Title = updateTaskDto.Title;
            task.Description = updateTaskDto.Description;
            task.Priority = updateTaskDto.Priority;
            task.UserId = updateTaskDto.UserId;

            await dbContext.SaveChangesAsync();

            // istersen updated TaskDto döndürebilirsin
            return Ok(new { message = "Güncellendi" });
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await dbContext.Tasks.FindAsync(id);
            if (task == null) return NotFound();

            dbContext.Tasks.Remove(task);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("priority")]
        public async Task<IActionResult> GetPriorityTasks()
        {
            var tasks = await dbContext.Tasks
                .Include(t => t.User)
                .OrderByDescending(t => t.Priority)
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    UserId = t.UserId,
                    UserName = t.User != null ? t.User.UserName : null,
                    Priority = t.Priority,
                    IsCompleted = t.IsCompleted,
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();

            return Ok(tasks);
        }
        [HttpGet("filterByDate")]
        public async Task<IActionResult> GetTasksByDateRange(DateTime startDate, DateTime endDate)
        {
            var tasks = await dbContext.Tasks
                .Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate)
                .Include(t => t.User)
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    UserId = t.UserId,
                    UserName = t.User != null ? t.User.UserName : null,
                    Priority = t.Priority,
                    IsCompleted = t.IsCompleted,
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();

            return Ok(tasks);
        }


        [HttpPatch("{id:int}/complete")]
        public async Task<IActionResult> MarkTaskAsCompleted(int id)
        {
            var task = await dbContext.Tasks.FindAsync(id);
            if (task == null)
                return NotFound("Görev bulunamadı");

            if (task.IsCompleted)
                return BadRequest("Görev zaten tamamlanmış.");
            task.IsCompleted = true;
            await dbContext.SaveChangesAsync();
            return Ok(new { message = "Görev tamamlandı olarak işaretlendi.", taskId = task.Id });

        }
    }
}
