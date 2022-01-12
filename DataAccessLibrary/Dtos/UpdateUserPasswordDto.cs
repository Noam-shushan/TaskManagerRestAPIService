using System.ComponentModel.DataAnnotations;

namespace TaskManager.DataAccess.Dtos
{
    /// <summary>
    /// Data transfer object to update the password of a user.
    /// </summary>
    public class UpdateUserPasswordDto
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
