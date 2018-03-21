using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaNation
{
    public class UserAuthentication : IUserAuthentication
    {
        private IDataBaseTable database;
        private IUser user;

        public UserAuthentication(IDataBaseTable database, IUser user)
        {
            this.database = database;
            this.user = user;
        }

        // Validates Email and password for login.
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

        // To access user name to display on GUI
        public string GetUserName()
        {
            return user.UserName;
        }
    }
}
