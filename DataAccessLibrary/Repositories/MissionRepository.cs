using TaskManager.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.DataAccess.Infrastructure;

namespace TaskManager.DataAccess.Repositories
{
    /// <summary>
    /// This is an implementation of the IModelRepository to access the database
    /// using crud operations on mission model
    /// </summary>
    public class MissionRepository : IModelRepository<Mission>
    {
        // Dependency injection of the data access
        readonly IDataAccess db;

        public MissionRepository(IDataAccess db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<Mission>> GetAllAsync()
        {
            try
            {
                string sql = @"select * 
                            from Mission 
                            where IsDeleted = false";
                var missions = await db.LoadDataAsync<Mission, dynamic>(sql, new { });
                return missions;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Mission>> GetAllAsync<U>(string propName, U value)
        {
            try
            {
                string sql = @$"select * 
                            from Mission 
                            where IsDeleted = false
                            and {propName} = @Value";
                
                var missions = await db.LoadDataAsync<Mission, dynamic>
                    (sql, new { Value = value });
                return missions;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Mission> GetByIdAsync(int id, bool getSimple = false)
        {
            try
            {
                string sql = @"select * 
                               from Mission 
                               where Id = @Id and IsDeleted = false";
                var mission = await db.LoadDataAsync<Mission, dynamic>(sql, new { Id = id });
                if (mission.Any())
                {
                    return mission.First();
                }
                throw new Exception($"Mission with id: {id} not found or deleted");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task Insert(Mission mission)
        {
            try
            {
                string sql = @"insert into 
                           mission (UserId, HeadLine, 
                                    Description, DateOfCreation, 
                                    Deadline, Status, 
                                    Priority, IsDeleted,
                                    ProgressStartDate, ProgressEndDate)
                           values (@UserId, @HeadLine, 
                                    @Description, @DateOfCreation, 
                                    @Deadline, @Status, 
                                    @Priority, @IsDeleted,
                                    @ProgressStartDate, @ProgressEndDate)";
                return db.SeveData(sql, mission);
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                throw new Exception($"User id : {mission.UserId} is not found int the users database");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task Update(Mission mission)
        {
            string sql = @"update Mission set
                           UserId = @UserId, 
                           HeadLine = @HeadLine, 
                           Description = @Description,
                           Deadline = @Deadline,
                           Status = @Status,
                           Priority = @Priority,
                           ProgressStartDate = @ProgressStartDate,
                           ProgressEndDate = @ProgressEndDate
                           where Id = @Id";
            try
            {
                return db.SeveData(sql, mission);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public Task Delete(int id)
        {
            try
            {
                string sql = @"update Mission set
                               IsDeleted = @IsDeleted where Id = @Id";
                return db.SeveData(sql, new { Id = id, IsDeleted = true });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Mission>> GetAllByAsync<U>(string sql, U parameters)
        {
            try
            {
                var filteruMissions = await db.LoadDataAsync<Mission, U>(sql, parameters);

                return filteruMissions;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> IsExist(int id)
        {
            try
            {
                string sql = @$"select Id from Mission where Id = @Id";

                var ids = await db.LoadDataAsync<int, dynamic>(sql, new { Id = id });

                return ids.Any();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task SeveAllBy<U>(string sql, U parameters)
        {
            try
            {
                return db.SeveData(sql, parameters);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
