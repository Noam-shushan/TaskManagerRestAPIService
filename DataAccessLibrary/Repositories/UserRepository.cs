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
    /// using crud operations on user model
    /// </summary>
    public class UserRepository : IModelRepository<User>
    {
        // The database access 
        readonly IDataAccess db;

        public UserRepository(IDataAccess db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<User>> GetAllByAsync<U>(string sql, U parameters)
        {
            try
            {
                var filteruUsers = await db.LoadDataAsync<User, U>(sql, parameters);

                return filteruUsers;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                string sql = @"select u.*, m.* 
                                from User u left join Mission m 
                                on u.Id = m.UserId
                                and u.IsDeleted = false and m.IsDeleted = false";
                var lookup = new Dictionary<int, User>();
                var usersTable = await db.LoadDataAsync<User, Mission>
                    // build the full user model with is all missions 
                    (sql, (user, mission) => 
                    {
                        if (!lookup.TryGetValue(user.Id, out User result))
                        {
                            lookup.Add(user.Id, result = user);
                        }
                        result.Missions ??= new();
                        if(mission != null)
                        {
                            result.Missions.Add(mission);
                        }
                        return result;
                    });
                return lookup.Values;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> GetByIdAsync(int id, bool getSimple = false)
        {
            string sql = @"select * 
                           from User 
                           where Id = @Id and IsDeleted = false";
            var user = await db.LoadDataAsync<User, dynamic>(sql, new { Id = id });
            if (user.Any())
            {
                var result = user.First();
                
                if (getSimple)
                {
                    return result;
                }

                result.Missions = await GetMissinsOfUserAsync(id);
                return result;
            }
            throw new Exception($"user with id: {id} not found or deleted");
        }

        public async Task Insert(User user)
        {
            try
            {
                // Check if this email is exists in the system
                // Get dynamic object with id and deleted flag
                var temp = await CheckEmailExists(user.Email);
                if(temp != null)
                {
                    if (temp.IsDeleted)
                    {   // If a deleted user tries to re-register
                        // we will just update him
                        user.Id = temp.Id;
                        await Update(user);
                        return;
                    }
                    // This email (username) exists in the system 
                    throw new Exception($"This {user.Email} is Email exists in the system");
                }

                string sql = @"insert into 
                               User (FirstName, LastName, 
                                       Email, RegistrationDate, 
                                       HashedPassword, IsDeleted) 
                               values (@FirstName, @LastName, 
                                       @Email, @RegistrationDate, 
                                       @HashedPassword, @IsDeleted)";
                await db.SeveData(sql, user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task Update(User user)
        {
            try
            {
                string sql = @"update User set 
                               FirstName = @FirstName, 
                               LastName = @LastName, 
                               Email = @Email,
                               HashedPassword = @HashedPassword,
                               IsDeleted = @IsDeleted
                               where Id = @Id";
                return db.SeveData(sql, user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task Delete(int id)
        {
            try
            {
                string sql = @"update User set IsDeleted = @IsDeleted where Id = @Id";
                return db.SeveData(sql, new { Id = id, IsDeleted = true });
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get all mission of some user to build the full user model 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<List<Mission>> GetMissinsOfUserAsync(int id)
        {
            string sql = @"select * 
                           from Mission
                           where UserId = @UserId and IsDeleted = false";
            var missions = await db.LoadDataAsync<Mission, dynamic>
                (sql, new { UserId = id });
            return missions.ToList();
        }

        /// <summary>
        /// Check if email is exist in the user table
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Anonimos object with id end deleted flag</returns>
        private async Task<dynamic> CheckEmailExists(string email)
        {
            string sql = @"select Id, IsDeleted
                           from User 
                           where Email = @Email";
            
            // Get dynamic object with id and deleted flag
            var idAndDeletetFlag = await db.LoadDataAsync<dynamic, dynamic>
                (sql, new { Email = email });

            if (idAndDeletetFlag.Any())
            {
                return idAndDeletetFlag.First();
            }
            return null;
        }

        public async Task<bool> IsExist(int id)
        {
            string sql = @$"select Id from User where Id = @Id";
            
            var ids = await db.LoadDataAsync<int, dynamic>(sql, new { Id = id });
            
            return ids.First() == id;
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

        public async Task<IEnumerable<User>> GetAllAsync<U>(string propName, U value)
        {
            try
            {
                string sql = @$"select *
                            where IsDeleted = false 
                            and u.{propName} = @Value";
                var filterdUsers = await db.LoadDataAsync<User, dynamic>
                    (sql, new { Value = value });
                return filterdUsers;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
