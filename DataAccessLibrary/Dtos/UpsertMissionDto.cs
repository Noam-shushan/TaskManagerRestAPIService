using TaskManager.DataAccess.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.DataAccess.Dtos
{
    /// <summary>
    /// Data transfer object to update or insert a new record of mission 
    /// </summary>
    public class UpsertMissionDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string HeadLine { get; set; }
        
        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Deadline { get; set; }

        [Required]
        [Range(1, 4)]
        public MissionStatus Status { get; set; }

        [Required]
        [Range(1, 3)]
        public Priority Priority { get; set; }
    }
}
