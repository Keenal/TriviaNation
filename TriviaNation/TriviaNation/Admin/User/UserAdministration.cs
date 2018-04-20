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
File Name: UserAdministration.cs 
*/

namespace TriviaNation
{
    /// <summary>
    /// A class to handle administrative tasks for user credentials
    /// </summary>
    public class UserAdministration : IUserAdministration
    {
        /// <summary>
        /// IUser object for modeling user data
        /// </summary>
        private IUser user;
        /// <summary>
        /// IDataBaseTable object for storing and retrieving user data
        /// </summary>
        private IDataBaseTable database;

        /// <summary>
        /// Constructs a UserAdministration object with default values as instance fields
        /// </summary>
        public UserAdministration()
        {
            this.user = null;
            this.database = null;
        }

        /// <summary>
        /// Constructs a UserAdministration object with database and user data as instance fields through use of interfaces 
        /// </summary>
        /// <param name="user">The user object</param>
        /// <param name="database">The database object related to users</param>
        public UserAdministration(IUser user, IDataBaseTable database)
        {
            this.user = user;
            this.database = database;
        }

        /// <summary>
        /// Adds user data to the database when creating a NEW user and confirms password
        /// </summary>
        /// <param name="userName">The user's username</param>
        /// <param name="email">The user's email</param>
        /// <param name="password">The user's password</param>
        /// <param name="confirmPassword">Confirms the user's password</param>
        /// <param name="score">The user's score</param>
        public Boolean AddUser(string userName, string email, string password, string confirmPassword, string score)
        {
            if (password.Equals(confirmPassword))
            {
                user.UserName = userName;
                user.Email = email;
                user.Password = password;
                user.Score = score;
                database.InsertRowIntoTable(database.TableName, this);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Deletes a user from the database
        /// </summary>
        /// <param name="userNumber">The username that matches the row position of a user</param>
        public void DeleteUser(int userNumber)
        {
            SetRowToObject(userNumber);
            database.DeleteRowFromTable(user.UserName);
        }

        private void SetRowToObject(int userNumber)
        {
            string tableRow = database.RetrieveTableRow(database.TableName, userNumber);
            string[] split = tableRow.Split(separator: '\n');
            user.UserName = split[0];
            user.Email = split[1];
            user.Password = split[2];
            user.Score = split[3];
        }

        /// <summary>
        /// Returns all users and their data in the database in the form of a string
        /// </summary>
        /// /// <returns>The list of users</returns>
        public IEnumerable<IUser> ListUsers()
        {
            List<IUser> allUserModels = new List<IUser>();
            for (int i = 1; i <= database.RetrieveNumberOfRowsInTable(); i++)
            {
                SetRowToObject(i);
                IUser userModel = new User
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Password = user.Password,
                    Score = user.Score
                };
                allUserModels.Add(userModel);
            }

            return allUserModels;
        }

      

        /// <summary>
        /// Returns a list of user properties/values
        /// </summary>
        /// <returns>The list of properties/values</returns>
        public IEnumerable<string> GetValues()
        {
            // string scoreToString = user.Score.ToString();
            List<string> userData = new List<string>
            {
                user.UserName, user.Email, user.Password, user.Score
            };

            return userData;
        }
    }
}
