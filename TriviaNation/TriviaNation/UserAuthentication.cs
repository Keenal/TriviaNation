using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
File Name: UserAuthentication.cs 
*/

namespace TriviaNation
{
    /// <summary>
    /// Class to aquire and authenticate user credentials
    /// </summary>
    public class UserAuthentication : IUserAuthentication
    {
        /// <summary>
        /// IDataBaseTable object for storing and retrieving user data
        /// </summary>
        private IDataBaseTable database;
        /// <summary>
        /// IUser object for modeling user data
        /// </summary>
        private IUser user;

        /// <summary>
        /// Constructs a UserAuthentication object with database and user objects as instance fields through use of IDataBaseTable and IUser interfaces 
        /// </summary>
        /// <param name="database">The database object related to users</param>
        /// <param name="user">The user object</param>
        public UserAuthentication(IDataBaseTable database, IUser user)
        {
            this.database = database;
            this.user = user;
        }

        /// <summary>
        /// Validates email and password for login and retrieves user data.
        /// </summary>
        /// <param name="email">The user's email</param>
        /// <param name="password">The user's password</param>
        /// <returns></returns>
        public Boolean AuthenticateUser(string email, string password)
        {
            string userData = "";
            string[] splitRow;
            for (int i = 1; i <= database.RetrieveNumberOfRowsInTable(); i++)
            {
                userData = database.RetrieveTableRow(database.TableName, i);
                splitRow = userData.Split(separator: '\n');
                if (splitRow[1].Equals(email) && splitRow[2].Equals(password))
                {
                    user.UserName = splitRow[0];
                    user.Email = splitRow[1];
                    user.Password = splitRow[2];
                    // May need to convert to int here in future.
                    user.Score = splitRow[3];
                    return true;
                }
            }

            return false;
        }

        // For future sprint.
        public string GetUserScore()
        {
            return "";
        }

        /// <summary>
        /// Used to access user name to display on GUI
        /// </summary>
        /// <returns>The user's username</returns>
        public string GetUserName()
        {
            return user.UserName;
        }
    }
}
