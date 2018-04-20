using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TriviaNation
{
    public interface IUserAuthentication
    {
        Boolean AuthenticateUser(string email, string password);
        string GetUserName();
        Boolean IsAdministrator();
    }
}
