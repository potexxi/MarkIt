using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MarkIt.login_register
{
    public class ClassUser
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public ClassUser(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
