using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
TriviaNation is a networked trivia game designed for use in
classrooms. Class members are each in control of a nation on
a map. The goal of the game is to increase the size of the nation 
by winning trivia challenges and defeating other class members 
in contested territories. The focus is on gamifying learning and
making it an enjoyable experience.

@author Timothy McWatters
@author Keenal Shah
@author Randy Quimby
@author Wesley Easton
@author Wenwen Xu
@version 1.0
CEN3032    "TriviaNation" SEII- Group 1's class project
File Name: User.cs 
*/

namespace TriviaNation
{
    /// <summary>
    /// Class to model user's usernames, emails, passwords and their scores.
    /// </summary>
    public class User : IUser
    {
        /// <summary>
        /// The user's username
        /// </summary>
        private string userName;
        /// <summary>
        /// The user's password
        /// </summary>
        private string password;
        /// <summary>
        /// The user's email
        /// </summary>
        private string email;
        /// <summary>
        /// The user's score
        /// </summary>
        private string score;

        /// <summary>
        /// Constructs a User object with default values as instance fields
        /// </summary>
        public User()
        {
            this.userName = "";
            this.password = "";
            this.email = "";
            this.score = "";
        }

        /// <summary>
        /// Accessor and mutator Property for the user's username
        /// </summary>
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
            }
        }

        /// <summary>
        /// Accessor and mutator Property for the user's password
        /// </summary>
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }

        /// <summary>
        /// Accessor and mutator Property for the user's email
        /// </summary>
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
            }
        }

        /// <summary>
        /// Accessor and mutator Property for the user's score
        /// </summary>
        public string Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
            }
        }
    }
}
