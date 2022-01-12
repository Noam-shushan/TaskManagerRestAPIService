using System;

namespace TaskManager.DataAccess.Models
{
    /// <summary>
    /// Mission model
    /// </summary>
    public class Mission
    {
        /// <summary>
        /// The mission Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User id that responsible for the mission 
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The headline of the mission
        /// </summary>
        public string HeadLine { get; set; }

        /// <summary>
        /// The description of the mission
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The date of creating this mission
        /// </summary>
        public DateTime DateOfCreation { get; set; }

        /// <summary>
        /// The deadline of the mission
        /// </summary>
        public DateTime Deadline { get; set; }

        /// <summary>
        /// The status of the mission ('Pending', 'InProgress', 'Done', 'Canceled')
        /// </summary>
        public MissionStatus Status { get; set; }

        /// <summary>
        /// The priority of the mission ('Low', 'Middle', 'High')
        /// </summary>
        public Priority Priority { get; set; }

        /// <summary>
        /// Flag that determine if this mission is deleted
        /// we don't delete anything from the database
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// The date they actually started working on the mission 
        /// </summary>
        public DateTime ProgressStartDate { get; set; }

        /// <summary>
        /// The date when complete the mission
        /// </summary>
        public DateTime ProgressEndDate { get; set; }
    }

    //CREATE TABLE taskmanagerdb.mission (
    //  Id int (11) NOT NULL AUTO_INCREMENT,
    //  UserId int (11) NOT NULL,
    //  HeadLine varchar(255) DEFAULT NULL,
    //  Description varchar(2000) DEFAULT NULL,
    //  DateOfCreation datetime DEFAULT NULL,
    //  Deadline datetime DEFAULT NULL,
    //  Status enum ('Pending', 'InProgress', 'Done', 'Canceled') DEFAULT NULL,
    //  Priority enum ('Low', 'Middle', 'High') DEFAULT NULL,
    //  IsDeleted tinyint(1) DEFAULT NULL,
    //  ProgressStartDate datetime DEFAULT NULL,
    //  ProgressEndDate datetime DEFAULT NULL,
    //  PRIMARY KEY(Id)
    //)
//ENGINE = INNODB,
//CHARACTER SET latin1,
//COLLATE latin1_swedish_ci;

//    ALTER TABLE taskmanagerdb.mission
//    ADD CONSTRAINT FK_Mission_UserId FOREIGN KEY(UserId)
//REFERENCES taskmanagerdb.user(Id) ON DELETE CASCADE;
}
