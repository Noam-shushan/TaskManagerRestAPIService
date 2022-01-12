using TaskManager.DataAccess.Models;
using System;
using System.ComponentModel.DataAnnotations;


namespace TaskManager.DataAccess.Dtos
{
    /// <summary>
    /// Data transfer object to get the relevant properties of mission to the front 
    /// </summary>
    public class GetMissionDto
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string HeadLine { get; set; }

        public string Description { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime DateOfCreation { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Deadline { get; set; }

        public MissionStatus Status { get; set; }

        public Priority Priority { get; set; }

        public DateTime ProgressStartDate { get; set; }

        public DateTime ProgressEndDate { get; set; }

        [DataType(DataType.Time)]
        public string ProgressTime
        {
            get
            {
                 if(ProgressStartDate != DateTime.MinValue 
                    && ProgressEndDate != DateTime.MinValue)
                {
                    return (ProgressEndDate - ProgressStartDate).ToString();
                }
                return "";
            }
        }
    }
}
