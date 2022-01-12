using TaskManager.DataAccess.Models;
using TaskManager.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.DataAccess.Services
{
    /// <summary>
    /// Perform statistical calculations
    /// </summary>
    public class Statistics
    {
        readonly IModelRepository<Mission> _missionRepo;

        public Statistics(RepositoriesContainer repositories)
        {
            _missionRepo = repositories.MissionRepository;
        }

        /// <summary>
        /// Get the average time to complete a mission
        /// </summary>
        /// <returns>TimeSpan that contain the average time.</returns>
        public async Task<TimeSpan> AverageTimeToCompletMission()
        {
            try
            {
                // Get all missions
                var missions = await _missionRepo
                    .GetAllAsync(propName: "Status", propValue: MissionStatus.Done);

                if (!missions.Any())
                {
                    return TimeSpan.MinValue;
                }

                var completMissionTimes = CompletMissionsTimes(missions);

                return GetAverageTimeSpan(completMissionTimes);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get the user who completes hes missions fast then every one else
        /// </summary>
        /// <returns>Anonymous object with user id and his average time to complete a mission</returns>
        public async Task<dynamic> GetUserWhoCompletesHisMissionsFastest()
        {
            try
            {
                // Get all missions
                var missions = await _missionRepo
                    .GetAllAsync(propName: "Status", propValue: MissionStatus.Done);

                if (!missions.Any())
                {
                    return null;
                }

                var result = (from m in missions
                                  // Make sure the mission is really complete
                              where m.ProgressStartDate != DateTime.MinValue
                              && m.ProgressEndDate != DateTime.MinValue
                              // The total time of complete mission 
                              let progressTime = m.ProgressEndDate - m.ProgressStartDate
                              // Group the progress time by the user id
                              group progressTime by m.UserId into times
                              // The average time of this user to finish is mission
                              let completAvg = GetAverageTimeSpan(times)
                              orderby completAvg
                              select new
                              {
                                  UserId = times.Key,
                                  AverageTimeToComplet = completAvg
                              }).First(); // The first one is the user
                                          // with the minimum average time to complete a mission
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Get user with most closed missions
        /// </summary>
        /// <returns></returns>
        public async Task<dynamic> GetUserWithMostClosedMissions()
        {
            try
            {
                // Get all missions
                var missions = await _missionRepo
                    .GetAllAsync(propName: "Status", propValue: MissionStatus.Done);

                if (!missions.Any())
                {
                    return null;
                }

                var result = (from m in missions
                              // missions by user id
                              group m by m.UserId into missionsOfUser
                              orderby missionsOfUser.Count() descending
                              select new
                              {
                                  UserId = missionsOfUser.Key,
                                  NumberOfClosedMission = missionsOfUser.Count()
                              }).First();// The first one is the user
                                         // with the maximum closed missions

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get average time of list of TimeSpans
        /// </summary>
        /// <param name="timeSpans"></param>
        /// <returns>The average time</returns>
        private TimeSpan GetAverageTimeSpan(IEnumerable<TimeSpan> timeSpans)
        {
            var avgOnTicks = timeSpans.Average(t => t.Ticks);
            var longAverageTicks = Convert.ToInt64(avgOnTicks);
            return new TimeSpan(longAverageTicks);
        }

        /// <summary>
        /// Helper method to get list of the time to complete a mission for all missions 
        /// </summary>
        /// <param name="missions"></param>
        /// <returns></returns>
        private static IEnumerable<TimeSpan> CompletMissionsTimes(IEnumerable<Mission> missions)
        {
            return from m in missions
                   // Make sure the mission is really complete
                   where m.ProgressStartDate != DateTime.MinValue
                   && m.ProgressEndDate != DateTime.MinValue
                   // The total time of complete mission 
                   select m.ProgressEndDate - m.ProgressStartDate;
        }
    }
}
