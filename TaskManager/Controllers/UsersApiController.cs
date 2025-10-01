using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.DTOs;
using TaskManager.Models;
namespace TaskManager.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersApiController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        public UsersApiController(AppDbContext appDbContext)
        {
            this.dbContext = appDbContext;

        }
        [HttpGet]
        public async Task<IActionResult> getAllUser()
        {
            var allUsers = await dbContext.Users
                .Include(u => u.Tasks)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    Tasks = u.Tasks.Select(t => new TaskDto
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Description = t.Description,
                        IsCompleted = t.IsCompleted,
                        Priority = t.Priority,
                        UserId=t.UserId,
                        CreatedAt = t.CreatedAt,
                        UserName = u.UserName
                    }).ToList()
                })
                .ToListAsync();

            return Ok(allUsers);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> getUsersById(int id)
        {
            var user = await dbContext.Users
                .Include(u => u.Tasks)
                .Where(u => u.Id == id)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    Tasks = u.Tasks.Select(t => new TaskDto
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Description = t.Description,
                        IsCompleted = t.IsCompleted,
                        Priority = t.Priority,
                        UserId=t.UserId,
                        CreatedAt = t.CreatedAt,
                        UserName = u.UserName
                    }).ToList()
                })
                .FirstOrDefaultAsync();
             
            //.SingleOrDefault();
            if (user is null)
            {
                return NotFound();
            }
            return Ok(user);

        }        

        [HttpPost]
        public async Task<IActionResult> createUser(UserDto request)
        {
            // Email kontrolü
            var userExists = await dbContext.Users.AnyAsync(u => u.Email == request.Email);
            if (userExists)
                return BadRequest("Bu email zaten kullanılıyor.");

            // Yeni User oluştur
            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                Tasks = request.Tasks.Select(t => new TaskItem
                {
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    Priority = t.Priority
                }).ToList()
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Tasks = user.Tasks.Select(t => new TaskDto
                {
                    Title = t.Title,
                    Description = t.Description,
                    Priority = t.Priority,
                    IsCompleted = t.IsCompleted,
                    UserId = user.Id
                }).ToList()
            };

            return CreatedAtAction(nameof(getUsersById), new { id = user.Id }, userDto);
        }
                

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> deleteUser(int id)
        {
            var user = await dbContext.Users.FindAsync(id);
            if (user is null)
            {
                return NotFound();
            }
            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
