using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TriviaNation
{
    public interface IUserAdministration : IDataEntry
    {
        Boolean AddUser(string username, string email, string password, string confirmPassword, string score);

        void DeleteUser(int questionNumber);

        IEnumerable<IUser> ListUsers();

        string UpdateScore(string username, string currentScore, int pointValue);

        IList<string> BuildUserInfo();
    }
}
