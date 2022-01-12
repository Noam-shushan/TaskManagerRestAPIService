using TaskManager.DataAccess.Dtos;
using TaskManager.DataAccess.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace TaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MissionController : ControllerBase
    {
        // Dependency injection
        readonly ILogger<MissionController> _logger;
        
        readonly MissionService _missionService;

        readonly Statistics _statistics;

        public MissionController(ILogger<MissionController> logger, IServicesContainer mainService)
        {
            _logger = logger;
            _missionService = mainService.MissionServices;
            _statistics = mainService.Statistics;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMissions()
        {
            try
            {
                var missions = await _missionService.GetAllMissionsAsync();
                return Ok(missions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "In 'MissionController.GetAllMissions'");
                throw;
            }
        }

        [HttpGet("Get missions of user/{userId:int}")]
        public async Task<IActionResult> GetAllByUserId(int userId)
        {
            try
            {
                var missions = await _missionService.GetMissionsByUserIdAsync(userId);
                return Ok(missions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "In 'MissionController.GetAllByUserId'");
                throw;
            }
        }

        [HttpGet("Get open missions")]
        public async Task<IActionResult> GetOpenMissions()
        {
            try
            {
                var missions = await _missionService.GetOpenMissionsAsync();
                return Ok(missions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "In 'MissionController.GetOpenMissions'");
                throw;
            }
        }

        [HttpGet("Get open missions of user/{userId:int}")]
        public async Task<IActionResult> GetOpenMissions(int userId)
        {
            try
            {
                var missions = await _missionService.GetOpenMissionsAsync(userId);
                return Ok(missions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "In 'MissionController.GetOpenMissions'");
                throw;
            }
        }

        [HttpGet("Missions whose deadline is this week")]
        public async Task<IActionResult> GetMissionsOfThisWeek()
        {
            try
            {
                var missions = await _missionService.GetMissionsOfThisWeekAsync();
                return Ok(missions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "In 'MissionController.GetMissionsOfThisWeek'");
                throw;
            }
        }

        [HttpGet("Closed missions")]
        public async Task<IActionResult> GetClosedMissions()
        {
            try
            {
                var missions = await _missionService.GetClosedMissionsAsync();
                return Ok(missions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "In 'MissionController.GetClosedMissions'");
                throw;
            }
        }

        [HttpGet("Closed missions of user/{userId:int}")]
        public async Task<IActionResult> GetClosedMissions(int userId)
        {
            try
            {
                var missions = await _missionService.GetClosedMissionsAsync(userId);
                return Ok(missions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "In 'MissionController.GetClosedMissions'");
                throw;
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetMission(int id)
        {
            try
            {
                var mission = await _missionService.GetMissionAsync(id);
                return Ok(mission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "In 'MissionController.GetMission'");
                return NotFound(ex.Message);
            }
        }

        [HttpGet("Average time to complete mission")]
        public async Task<ActionResult> AverageTimeToCompletMission()
        {
            try
            {
                var avg = await _statistics.AverageTimeToCompletMission();
                if(avg != TimeSpan.MinValue)
                {
                    return Ok(avg);
                }
                return BadRequest("There is no completed missions");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "In 'MissionController.AverageTimeToCompletMission'");
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddMission(UpsertMissionDto missionDto)
        {
            try
            {
                await _missionService.AddMission(missionDto);
                return Ok("New mission successfully added");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "In 'MissionController.AddMission'");
                throw;
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateMission(int id, UpsertMissionDto missionDto)
        {
            try
            {
                await _missionService.UpdateMissionAsync(id, missionDto);
                return Ok("Mission successfully update");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "In 'MissionController.UpdateMission'");
                throw;
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteMission(int id)
        {
            try
            {
                await _missionService.DeleteMissionAsync(id);
                return Ok("Mission deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "In 'MissionController.DeleteMission'");
                throw;
            }
        }
    }
}
