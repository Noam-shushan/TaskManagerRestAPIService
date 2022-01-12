using System;
using System.Collections.Generic;

namespace TaskManager.DataAccess.Models
{
    /// <summary>
    /// User model
    /// </summary>
    public class User
    {
        /// <summary>
        /// The user id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Flag that determine if this user is deleted
        /// we don't delete anything from the database
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// The first name of the user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The Email user. also the username for login
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Hashed password 
        /// we don't save the real password
        /// </summary>
        public string HashedPassword { get; set; }

        /// <summary>
        /// Registration date to the system
        /// </summary>
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// Missions list of this user
        /// </summary>
        public List<Mission> Missions { get; set; }
    }

    //CREATE TABLE taskmanagerdb.user(
    //      Id int (11) NOT NULL AUTO_INCREMENT,
    //      FirstName varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
    //      LastName varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
    //      Email varchar(320) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
    //      IsDeleted tinyint(1) DEFAULT NULL,
    //      HashedPassword varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
    //      RegistrationDate varchar(255) DEFAULT NULL,
    //      PRIMARY KEY(Id)
    //    )
    //    ENGINE = INNODB,
    //    CHARACTER SET latin1,
    //    COLLATE latin1_swedish_ci;

    //    ALTER TABLE taskmanagerdb.user
    //    ADD UNIQUE INDEX Email(Email);
}
