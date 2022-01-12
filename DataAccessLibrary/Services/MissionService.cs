using TaskManager.DataAccess.Dtos;
using TaskManager.DataAccess.Tools;
using TaskManager.DataAccess.Models;
using TaskManager.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.DataAccess.Services
{
    /// <summary>
    /// Logical management of mission data
    /// </summary>
    public class MissionService
    {
        // Dependency injection of the mission repository
        // to communicate with the database
        readonly IModelRepository<Mission> _repository;

        public MissionService(IModelRepository<Mission> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get all missions from the database
        /// </summary>
        /// <returns>Collection of all the mission as MissionDTO's</returns>
        public async Task<IEnumerable<GetMissionDto>> GetAllMissionsAsync()
        {
            try
            {
                var missions = await _repository.GetAllAsync();
                return from m in missions
                       select m.CopyPropertiesToNew(typeof(GetMissionDto)) as GetMissionDto;
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Get single mission by id 
        /// </summary>
        /// <param name="id">The id of the mission to revise</param>
        /// <returns>The mission match to this id</returns>
        public async Task<GetMissionDto> GetMissionAsync(int id)
        {
            try
            {
                var mission = await _repository.GetByIdAsync(id);
                return mission.CopyPropertiesToNew(typeof(GetMissionDto)) as GetMissionDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Add new mission
        /// </summary>
        /// <param name="missionDto"></param>
        /// <returns>Task to await for</returns>
        public Task AddMission(UpsertMissionDto missionDto)
        {
            try
            {
                // Copy the proprietress from the DTO
                var mission = missionDto.CopyPropertiesToNew(typeof(Mission)) as Mission;

                // When adding a new mission we want to save
                // the start date of the active progress on the mission
                // or the date when the mission in done
                // (in case of adding a mission to the archive) 
                SetDateOfPrograssOnInsert(mission);
                
                // Save the data of creating this mission
                mission.DateOfCreation = DateTime.Now;
                
                // Insert to the database
                return _repository.Insert(mission);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Update mission
        /// </summary>
        /// <param name="id">The id of the mission</param>
        /// <param name="missionDto">The new data to update</param>
        /// <returns>Task to await for</returns>
        public async Task UpdateMissionAsync(int id, UpsertMissionDto missionDto)
        {
            try
            {
                // Get the mission from the database
                var mission = await _repository.GetByIdAsync(id);

                // On update mission we save the dates  to track the progress
                // of the mission
                SetDateOfProssingOnUpdate(missionDto, mission);

                // Copy the proprietress from the DTO
                missionDto.CopyPropertiesTo(mission);

                // Save to database
                await _repository.Update(mission);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Delete mission
        /// </summary>
        /// <param name="id">The id of the mission to delete</param>
        /// <returns></returns>
        public async Task DeleteMissionAsync(int id)
        {
            try
            {
                bool isExist = await _repository.IsExist(id);
                if (!isExist)
                {
                    throw new Exception($"Mission with id: {id} not found");
                }
                await _repository.Delete(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get all closing mission with an option to get closing missions of some user
        /// </summary>
        /// <param name="userId">Optional user id</param>
        /// <returns>Collection of closed missions </returns>
        public async Task<IEnumerable<GetMissionDto>> GetClosedMissionsAsync(int userId = 0)
        {
            try
            {
                string sqlGetOfUser = @"select * 
                                       from Mission
                                       where UserId = @UserId 
                                       and IsDeleted = false
                                       and Status = @Done
                                       order by Deadline";
                string sqlGetAll = @"select * 
                                    from Mission
                                    where IsDeleted = false
                                    and Status = @Done
                                    order by Deadline";

                var sql = userId == 0 ? sqlGetAll : sqlGetOfUser;

                var missionsOfUser = await _repository.GetAllByAsync(sql,
                    new
                    {
                        UserId = userId,
                        Done = MissionStatus.Done,
                    });

                return from m in missionsOfUser
                       select m.CopyPropertiesToNew(typeof(GetMissionDto)) as GetMissionDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get missions of user by user id sorted by the deadline of the missions
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>Collection of missions of the user match to this id</returns>
        public async Task<IEnumerable<GetMissionDto>> GetMissionsByUserIdAsync(int userId)
        {
            try
            {
                string sql = @"select * 
                           from Mission
                           where UserId = @UserId and IsDeleted = false
                           order by Deadline";
                
                var missionsOfUser = await _repository
                    .GetAllByAsync(sql, new { UserId = userId });

                return from m in missionsOfUser
                       select m.CopyPropertiesToNew(typeof(GetMissionDto)) as GetMissionDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get all mission of this week
        /// </summary>
        /// <returns>Collection of missions that her deadline is in this week</returns>
        public async Task<IEnumerable<GetMissionDto>> GetMissionsOfThisWeekAsync()
        {
            try
            {
                string sql = @"select * 
                           from Mission
                           where IsDeleted = false
                           and  YEARWEEK(Deadline, 1) = YEARWEEK(CURDATE(), 1)
                           order by Deadline";
                
                var missionsOfUser = await _repository.GetAllByAsync(sql, new { });

                return from m in missionsOfUser
                       select m.CopyPropertiesToNew(typeof(GetMissionDto)) as GetMissionDto;
            }
            catch (Exception)
            {
                throw;
            };
        }

        /// <summary>
        /// Get all open mission with option to get open mission of some user
        /// </summary>
        /// <param name="userId">Optional user id</param>
        /// <returns></returns>
        public async Task<IEnumerable<GetMissionDto>> GetOpenMissionsAsync(int userId = 0)
        {
            try
            {
                string sqlGetOfUser = @"select * 
                                       from Mission
                                       where UserId = @UserId 
                                       and IsDeleted = false
                                       and not (Status = @Done or Status = @Canceled)
                                       order by Deadline";
                string sqlGetAll = @"select * 
                                    from Mission
                                    where IsDeleted = false
                                    and not (Status = @Done or Status = @Canceled)
                                    order by Deadline";
                
                var sql = userId == 0 ? sqlGetAll : sqlGetOfUser;
                
                var missionsOfUser = await _repository.GetAllByAsync(sql, 
                    new 
                    { 
                        UserId = userId, 
                        Done = MissionStatus.Done, 
                        Canceled = MissionStatus.Canceled 
                    });

                return from m in missionsOfUser
                       select m.CopyPropertiesToNew(typeof(GetMissionDto)) as GetMissionDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///  When adding a new mission we want to save
        ///  the start date of the active progress on the mission
        ///  or the date when the mission in done (in case of adding a mission to the archive)
        /// </summary>
        /// <param name="mission">The new mission</param>
        private void SetDateOfPrograssOnInsert(Mission mission)
        {
            if (mission.Status == MissionStatus.InProgress)
            {
                mission.ProgressStartDate = DateTime.Now;
            }
            else if (mission.Status == MissionStatus.Done)
            {
                mission.ProgressEndDate = DateTime.Now;
            }
        }

        /// <summary>
        /// On update mission we save the dates  to track the progress of the mission
        /// </summary>
        /// <param name="missionDto"></param>
        /// <param name="mission"></param>
        private void SetDateOfProssingOnUpdate(UpsertMissionDto missionDto, Mission mission)
        {
            if (mission.Status != MissionStatus.InProgress
                && missionDto.Status == MissionStatus.InProgress)
            {
                mission.ProgressStartDate = DateTime.Now;
            }
            else if (mission.Status == MissionStatus.InProgress
                && missionDto.Status == MissionStatus.Done)
            {
                mission.ProgressEndDate = DateTime.Now;
            }
        }
    }
}
