using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using User.Domain.Repositories;
using User.Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly DataContext _context;

        public RoleController(DataContext context)
        {
            _context = context;
        }

        // POST: api/role/assign
        [HttpPost("assign")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AssignRole([FromBody] RoleAssignmentDto dto)
        {
            var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == dto.UserId);
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == dto.RoleId);
            if (user == null || role == null)
                return NotFound("Użytkownik lub rola nie istnieje");

            if (user.Roles == null)
                user.Roles = new List<Role>();
            if (!user.Roles.Any(r => r.Id == role.Id))
                user.Roles.Add(role);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // POST: api/role/remove
        [HttpPost("remove")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> RemoveRole([FromBody] RoleAssignmentDto dto)
        {
            var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == dto.UserId);
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == dto.RoleId);
            if (user == null || role == null)
                return NotFound("Użytkownik lub rola nie istnieje");

            if (user.Roles != null && user.Roles.Any(r => r.Id == role.Id))
            {
                user.Roles.Remove(user.Roles.First(r => r.Id == role.Id));
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        // DELETE: api/role/user/{userId}
        [HttpDelete("user/{userId}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound();
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }

    public class RoleAssignmentDto
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
} 