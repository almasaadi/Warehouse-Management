using ManagmentSystem.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagmentSystem.Exceptions;

namespace ManagmentSystem.Services
{
    public class UserValidationService : IUserValidationService
    {
        public void ValidateLoginInput(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new AuthenticationException("Login Error: Username and Password fields are required.");
        }
    }
}
