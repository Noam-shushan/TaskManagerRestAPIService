using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TaskManager.DataAccess.Services;
using TaskManager.DataAccess.Dtos;

namespace TaskManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        // Dependency injection
        readonly ILogger<UserController> _logger;

        readonly UserService _userService;
        readonly Statistics _statistics;

        public UserController(ILogger<UserController> logger, IServicesContainer mainService)
        {
            _logger = logger;
            _userService = mainService.UserServices;
            _statistics = mainService.Statistics;
        }

        [HttpGet("Users")]
        public async Task<ActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "In 'UserController.GetAllUsers'");
                throw;
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetUser(int id)
        {
            try
            {
                var user = await _userService.GetUser(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "In 'UserController.GetUser'");
                return NotFound(ex.Message);
            }
        }

        [HttpGet("Get user with most closed missions")]
        public async Task<ActionResult> GetUserWithMostClosedMissions()
        {
            try
            {
                var result = await _statistics.GetUserWithMostClosedMissions();
                if(result != null)
                {
                    return Ok(result);
                }
                return BadRequest("There are no closed missions");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "In 'UserController.GetUser'");
                return NotFound(ex.Message);
            }
        }

        [HttpGet("Get the user who finishes his tasks the fastest")]
        public async Task<ActionResult> GetUserWhoCompletesHisMissionsFastest()
        {
            try
            {
                var result = await _statistics.GetUserWhoCompletesHisMissionsFastest();
                if (result != null)
                {
                    return Ok(result);
                }
                return BadRequest("There are no closed missions");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "In 'UserController.GetUserWhoCompletesHisMissionsFastest'");
                return NotFound(ex.Message);
            }
        }

        [HttpGet("Login/{email},{password}")]
        public async Task<ActionResult> Login(string email, string password)
        {
            try
            {
                bool isValidUser = await _userService.Login(email, password);

                return isValidUser ? Ok("User successfully login")
                    : NotFound("Incorrect password or email");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("Register")]
        public async Task<ActionResult> SignUp(CreateUserDto newUser)
        {
            try
            {
                await _userService.SignUp(newUser);
                return Ok("New user successfully added");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "In 'UserController.Insert'");
                throw;
            }
        }

        [HttpPut("Update/{id:int}")]
        public async Task<ActionResult> UpdateUser(int id, UpdateUserDto updateUserDto)
        {
            try
            {
                await _userService.UpdateUser(id, updateUserDto);
                return Ok("User update successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "In 'UserController.UpdateUser'");
                throw;
            }
        }

        [HttpPut("Update password")]
        public async Task<ActionResult> UpdatePassword(UpdateUserPasswordDto passwordDto)
        {
            try
            {
                await _userService.UpdatePassword(passwordDto);
                return Ok("User password update successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "In 'UserController.UpdatePassword'");
                throw;
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUser(id);
                return Ok("User deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "In 'UserController.Delete'");
                throw;
            }
        }
    }
}
