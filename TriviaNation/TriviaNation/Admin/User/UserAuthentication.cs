using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TriviaNation.Repository.Abstract;

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
        private static readonly int CorrectAnswerPoints = 5;
        /// <summary>
        /// IDataBaseTable object for storing and retrieving user data
        /// </summary>
        private IUserTable database;
        /// <summary>
        /// IUser object for modeling user data
        /// </summary>
        private IUser user;

        public UserAuthentication()
        {
            this.database = null;
            this.user = null;
        }

        /// <summary>
        /// Constructs a UserAuthentication object with database and user objects as instance fields through use of IDataBaseTable and IUser interfaces 
        /// </summary>
        /// <param name="database">The database object related to users</param>
        /// <param name="user">The user object</param>
        public UserAuthentication(IUserTable database, IUser user)
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
                    user.Score = splitRow[3];
                    return true;
                }
            }

            return false;
        }

        public Boolean IsAdministrator()
        {
            if (user.Email.Equals("teacher"))
            {
                return true;
            }
            return false;
        }

        public void ComputeScore()
        {
            int convertedScore = Convert.ToInt32(user.Score);
            convertedScore = convertedScore + CorrectAnswerPoints;
            user.Score = convertedScore.ToString();
            SaveScoreToDatabase();
        }

        public void SaveScoreToDatabase()
        {
            IUserAdministration admin = new UserAdministration();
            for (int i = 0; i < database.RetrieveNumberOfRowsInTable(); i++)
            {
                string tableRow = database.RetrieveTableRow(database.TableName, i);
                string[] split = tableRow.Split(separator: '\n');

                if (user.UserName.Equals(split[0]))
                {
                   admin.DeleteUser(i);
                   admin.AddUser(user.UserName, user.Email, user.Password, user.Password, user.Score);
                }
            }
        }

        // To return user object affiliated with authentication
        public IUser GetUserData()
        {
            return user;
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
