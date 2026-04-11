using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KP_SnakeProjekt.Models
{
    public class Users
    {
        public string UserName {get;set;} 
        public int Id {get;set;}
        public string Password {get;set; }

        public Users() { }

        public Users(int id, string userName, string password)
        {
            Id = id;
            UserName = userName;
            Password = password;
        }
    }
}
