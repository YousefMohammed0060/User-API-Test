using API_Test.Data;
using API_Test.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UsersController(AppDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
            return Ok(await _context.users.ToArrayAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetUser(int id)
        {
            var user = await _context.users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<Users>> PostUser(Users user)
        {
            await _context.users.AddAsync(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                "GetUser",
                new { id = user.Id },
                user);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutUser(int id, Users user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.users.Any(p => p.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Users>> DeleteUser(int id)
        {
            var user = await _context.users.FindAsync(id);
            if (user == null) { return NotFound(); }

            _context.users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }


        [HttpPost]
        [Route("Delete")]
        public async Task<ActionResult> DeleteMultiple([FromQuery] int[] ids)
        {
            var users = new List<Users>();
            foreach (var id in ids)
            {
                var user = await _context.users.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                users.Add(user);
            }

            _context.users.RemoveRange(users);
            await _context.SaveChangesAsync();

            return Ok(users);
        }
    }
}
