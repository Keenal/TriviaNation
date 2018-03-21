using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaNation
{
    public class UserAdministration : IUserAdministration
    {
        private IUser user;
        private IDataBaseTable database;

        public UserAdministration()
        {
            this.user = null;
            this.database = null;
        }

        public UserAdministration(IUser user, IDataBaseTable database)
        {
            this.user = user;
            this.database = database;
        }

        // This is used to add user data to the database when creating a NEW user.
        // This is where a username will be produced from the email if thats how we want to do it
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

        // This may not be used but I implemented it anyways.  Deletes a user's data
        public void DeleteUser(string userNumber)
        {
            String tableRow = database.RetrieveTableRow(database.TableName, userNumber);
            String[] split = tableRow.Split(separator: '\n');
            user.UserName = split[0];
            database.DeleteRowFromTable(user.UserName);
        }

        // Used to list users so that current user can choose one to delete (administration)
        // Also can be used for many other things of course.
        public string ListUsers()
        {
            string listOfUsers = "";
            for (int i = 1; i <= database.RetrieveNumberOfRowsInTable(); i++)
            {
                string marker = "cx" + i;
                listOfUsers = listOfUsers + i + ". " + database.RetrieveTableRow(database.TableName, marker);
            }

            return listOfUsers;
        }

        // Dont worry about this method.  Its for Tim's (database) use.
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
