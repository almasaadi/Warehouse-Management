using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagmentSystem.Enums;

namespace ManagmentSystem.Models
{
    public class Employee : User
    {
        public Employee(string username, string password) : base(username, password)
        {
            Role= UserRole.Employee;
        }
        public PersonalInfo  PersonalInfo { get; set; } = new();

    }
}
