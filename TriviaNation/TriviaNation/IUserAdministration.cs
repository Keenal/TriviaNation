using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TriviaNation
{
    public interface IUserAdministration : IDataEntry
    {
        // This can be combined with ITriviaAdministration (rename it administration)
        Boolean AddUser(string username, string email, string password, string confirmPassword, string score);
        void DeleteUser(int questionNumber);
        string ListUsers();
    }
}
