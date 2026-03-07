using ManagmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Services
{
    public static class UserSession
    {
        public static Employee? CurrentEmployee { get; set; }
    }
}
