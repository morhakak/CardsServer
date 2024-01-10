using CardsServer.Exceptions;
using CardsServer.Models.UserModel;
using CardsServer.Services.Users;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CardsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _usersService;

        public UsersController(IMongoClient mongoClient)
        {
            _usersService = new UsersService(mongoClient);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _usersService.GetUsersAsync();
            return Ok(users);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(string userId)
        {
            var user = await _usersService.GetUserAsync(userId);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var results = await _usersService.DeleteUserAsync(userId);

            if (results is false)
            {
                return StatusCode(500, "Failed to delete user - try again");
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User newUser)
        {
            try
            {
                await _usersService.CreateUserAsync(newUser);
            }
            catch (UserAlreadyExistsException)
            {

                return Conflict($"User with {newUser.Email} email already exits");
            }

            return CreatedAtAction(nameof(CreateUser), new { userId = newUser.Id }, newUser);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> Put(string id, [FromBody] User updatedUser)
        {
            var results = await _usersService.UpdateUserAsync(id, updatedUser);

            if (results is false)
            {
                return StatusCode(500, "Failed to update user - try again");
            }

            return NoContent();
        }
    }
}
