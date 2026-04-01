using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkIt.login_register
{
    public class ClassUserList
    {
        public List<ClassUser> Users { get; set; }
        public ClassUserList() 
        {
            Users = new List<ClassUser>();
        }
    }
}
