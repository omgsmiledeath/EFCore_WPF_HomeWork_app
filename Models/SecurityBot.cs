using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore_WPF_HomeWork_app.Models
{
    internal class SecurityBot
    {
        private readonly string login;
        private readonly string password;

        public SecurityBot()
        {
            login = "Skillbox";
            password = "C#pro";
        }
        public bool TryToLogin(string login, string password)
        {
            if (this.login.Equals(login) && this.password.Equals(password))
            {
                return true;
            }
            else return false;
        }
    }

}
