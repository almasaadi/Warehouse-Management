using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ManagmentSystem.Contracts;
using ManagmentSystem.Exceptions;
namespace ManagmentSystem.Services
{
    public class EmployeeValidationService : IEmployeeValidationService
    {
        public void ValidatePersonalData(string username, string password, string phone)
        {
            if (!Regex.IsMatch(username, @"^[a-zA-Z][a-zA-Z0-9]{3,11}$"))
                throw new InvalidInputException("Invalid Username: Must start with a letter and be 4-12 characters long.");

            // Password: Min 8 chars, at least one uppercase, one lowercase, and one number
            if (!Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"))
                throw new InvalidInputException("Weak Password: Must contain at least one uppercase letter, one lowercase letter, and one number.");

            string phonePattern = @"^(\+9639\d{8}|09\d{8})$";
            if (!Regex.IsMatch(phone, phonePattern))
                throw new InvalidInputException("Invalid Syrian phone format. Use +9639xxxxxxxx or 09xxxxxxxx.");

            if (string.IsNullOrWhiteSpace(phone))
                throw new InvalidInputException("Phone number is required.");

        }
    }
}
