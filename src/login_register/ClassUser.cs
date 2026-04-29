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
        private int id;
        public int Id 
        {
            get
            {
                return id;
            }
            set
            {
                if (value < -1)
                {
                    throw new ArgumentException("negativ id");
                }
                else
                {
                    id = value;
                }
            } 
        }
        public string Email { get; set; }
        public string Password { get; set; }

        public ClassUser(int id, string email, string password)
        {
            Id = id;
            Email = email;
            Password = password;
        }
    }
}
