using TaskManager.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.DataAccess.Dtos
{
    /// <summary>
    /// Data transfer object to get the relevant properties of user to the front 
    /// </summary>
    public class GetUserDto
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime RegistrationDate { get; set; }

        public List<Mission> Missions { get; set; }
    }
}
