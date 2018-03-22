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
            // score needs to be converted to int or made into an int and converted to string 
            // for future sprint though.
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
            String tableRow = database.RetrieveTableRow(database.TableName, userNumber);
            String[] split = tableRow.Split(separator: '\n');
            user.UserName = split[0];
            database.DeleteRowFromTable(user.UserName);
        }

        /// <summary>
        /// Returns all users and their data in the database in the form of a string
        /// </summary>
        /// /// <returns>The list of users</returns>
        public string ListUsers()
        {
            string listOfUsers = "";
            for (int i = 1; i <= database.RetrieveNumberOfRowsInTable(); i++)
            {
                listOfUsers = listOfUsers + i + ". " + database.RetrieveTableRow(database.TableName, i);
            }

            return listOfUsers;
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
