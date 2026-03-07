using ManagmentSystem.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Models
{
    public class Manager : Employee
    {
        public Manager(string username, string password) : base(username, password)
        {
            Role = UserRole.Manager;
        }

    }
}
