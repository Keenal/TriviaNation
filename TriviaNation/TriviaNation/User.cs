using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaNation
{
    public class User : IUser
    {
        private string userName;
        private string password;
        private string email;
        private string score;

        public User()
        {
            this.userName = "";
            this.password = "";
            this.email = "";
            this.score = "";
        }

        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }

        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
            }
        }

        public string Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
            }
        }
    }
}
