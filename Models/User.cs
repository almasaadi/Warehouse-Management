using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagmentSystem.Enums;
using System.Threading.Tasks;
using ManagmentSystem.Models;

namespace ManagmentSystem
{
    public abstract class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        
        protected User( string username, string password)
        {
            Username = username;
            Password = password;
            
        }
    }
}
