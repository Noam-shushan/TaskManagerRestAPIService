using System.ComponentModel.DataAnnotations;

namespace TaskManager.DataAccess.Dtos
{
    /// <summary>
    /// Data transfer object to insert a new record of user.
    /// is expands the UpdateUserDto to transfer also the password
    /// </summary>
    public class CreateUserDto : UpdateUserDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
