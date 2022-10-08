using Microsoft.AspNetCore.Mvc;
using UsersNG.Models;
using UsersNG.Services.UserService;
using UsersNG.Shared;

namespace UsersNG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _userService.GetUser();
            return Ok(result);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUser(id);

            if (!user.Success)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            var result = await _userService.PutUser(id, user);

            if (!result.Success)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostUser(User user)
        {
            var result = await _userService.PostUser(user);

            if (!result.Success)
            {
                return Conflict();
            }

            return CreatedAtAction("GetUser", new { id = result?.Data?.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userService.DeleteUser(id);
            if (!user.Success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
