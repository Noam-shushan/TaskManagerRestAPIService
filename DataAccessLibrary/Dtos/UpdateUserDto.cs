using System.ComponentModel.DataAnnotations;

namespace TaskManager.DataAccess.Dtos
{
    /// <summary>
    /// Data transfer object to update record of user 
    /// </summary>
    public class UpdateUserDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}


