using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaNation
{
    public interface IUserAuthentication
    {
        Boolean AuthenticateUser(string userName, string password);
        string GetUserName();
    }
}
