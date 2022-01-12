using TaskManager.DataAccess.Dtos;
using TaskManager.DataAccess.Models;
using TaskManager.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DataAccess.Tools;

namespace TaskManager.DataAccess.Services
{
    /// <summary>
    /// Logical managmet of the users data
    /// </summary>
    public class UserService
    {
        readonly IModelRepository<User> _repository;

        public UserService(IModelRepository<User> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get all users as full model object
        /// </summary>
        /// <returns>All users</returns>
        public async Task<IEnumerable<GetUserDto>> GetAllUsers()
        {
            try
            {
                var users = await _repository.GetAllAsync();
                return from user in users
                       select user.CopyPropertiesToNew(typeof(GetUserDto)) as GetUserDto;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get single user by user id
        /// </summary>
        /// <param name="id">The user id</param>
        /// <returns>Single user match to this id</returns>
        public async Task<GetUserDto> GetUser(int id)
        {
            try
            {
                var user = await _repository.GetByIdAsync(id);
                return user.CopyPropertiesToNew(typeof(GetUserDto)) as GetUserDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Signup a new user to the system
        /// </summary>
        /// <param name="newUserDto"></param>
        /// <returns></returns>
        public async Task SignUp(CreateUserDto newUserDto)
        {
            try
            {   // Copies the necessary properties 
                var user = newUserDto.CopyPropertiesToNew(typeof(User)) as User;
                
                // Get Hashed password, We will not save the real password  
                user.HashedPassword = GetHashPassword(newUserDto.Password);
                user.RegistrationDate = DateTime.Now;
                
                // Insert new user to the database
                await _repository.Insert(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Update password of user
        /// </summary>
        /// <param name="passwordDto"></param>
        /// <returns></returns>
        public async Task UpdatePassword(UpdateUserPasswordDto passwordDto)
        {
            try
            {
                var user = await _repository.GetByIdAsync(passwordDto.Id, getSimple: true);
                if (user is null)
                {
                    throw new Exception($"user with id: {passwordDto.Id} not found");
                }

                // Encrypts the new password
                user.HashedPassword = GetHashPassword(passwordDto.Password);

                // Update the user in the database
                await _repository.Update(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="id">id match to the user</param>
        /// <param name="updateUserDto"></param>
        /// <returns></returns>
        public async Task UpdateUser(int id, UpdateUserDto updateUserDto)
        {
            try
            {
                var user = await _repository.GetByIdAsync(id, getSimple:true);
                if (user is null)
                {
                    throw new Exception($"user with id: {id} not found");
                }

                // Copies the necessary properties
                updateUserDto.CopyPropertiesTo(user);
                
                // Update the user in the database
                await _repository.Update(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteUser(int id)
        {
            try
            {
                bool isExist = await _repository.IsExist(id);
                if (!isExist)
                {
                    throw new Exception($"User with id: {id} not found");
                }

                // delete all the missions of the user
                string sql = @"update Mission set
                               IsDeleted = @IsDeleted where UserId = @UserId";  
                await _repository.SeveAllBy<dynamic>(sql, 
                    new { IsDeleted = true, UserId = id });

                await _repository.Delete(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Login. verify the username (email addres) match to the password 
        /// </summary>
        /// <param name="email">The username</param>
        /// <param name="password">The password</param>
        /// <returns></returns>
        public async Task<bool> Login(string email, string password)
        {
            try
            {
                string sql = @"select * 
                           from User 
                           where Email = @Email";
                var users = await _repository.GetAllByAsync(sql, new { Email = email });

                var userToVerify = users.FirstOrDefault(
                    u => u.HashedPassword == GetHashPassword(password));

                return userToVerify != null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get the hash password of a given real password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        static string GetHashPassword(string password)
        {
            SHA512 shaM = new SHA512Managed();
            return Convert.ToBase64String(shaM.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }


    }
}
