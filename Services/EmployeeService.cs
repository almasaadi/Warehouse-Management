using System;
using System.Collections.Generic;
using System.Linq;
using ManagmentSystem.Models;
using ManagmentSystem.Contracts;
using ManagmentSystem.Services;
using ManagmentSystem.Data;
using ManagmentSystem.Exceptions;

namespace ManagmentSystem.Services
{
    public class EmployeeService : IEmployeeManager
    {
        private readonly IEmployeeValidationService _validationService;
        private readonly JsonHelper<Employee> _jsonHelper;
        private List<Employee> Employees;

        public EmployeeService()
        {
            _validationService = new EmployeeValidationService();
            _jsonHelper = new JsonHelper<Employee>("employees.json");

            Employees = _jsonHelper.Load();

            if (Employees.Count == 0)
            {
                InitializeSeedData();
            }
        }

        private void InitializeSeedData()
        {
            var admin = new Manager("admin", "Admin123")
            {
                Id = 1, 
                PersonalInfo = new PersonalInfo
                {
                    FirstName = "System",
                    LastName = "Admin",
                    PhoneNumber = "0912345678"
                }
            };

            var staff = new Employee("user", "User123")
            {
                Id = 2,
                PersonalInfo = new PersonalInfo
                {
                    FirstName = "Normal",
                    LastName = "Employee",
                    PhoneNumber = "0987654321"
                }
            };

            Employees.Add(admin);
            Employees.Add(staff);

            _jsonHelper.Save(Employees);
        }

        public void AddEmployee(Employee newEmp)
        {
            if (newEmp.PersonalInfo.PhoneNumber == null)
                throw new InvalidInputException("Personal information is required.");

            _validationService.ValidatePersonalData(
                newEmp.Username,
                newEmp.Password,
                newEmp.PersonalInfo.PhoneNumber
            );


            if (Employees.Any(e =>
                e.Username.Equals(newEmp.Username, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception("Username already exists.");
            }

            newEmp.Id = Employees.Count > 0
                ? Employees.Max(e => e.Id) + 1
                : 1;


            Employees.Add(newEmp);
            _jsonHelper.Save(Employees);
        }

        public void RemoveEmployee(string username)
        {
            var target = Employees.FirstOrDefault(e =>
                e.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (target == null)
                throw new InvalidInputException("Employee not found.");


            Employees.Remove(target);
            _jsonHelper.Save(Employees);
        }

        public List<Employee> GetAllEmployees()
        {
            return Employees.ToList();
        }

        public void UpdateEmployee(string username, Employee updated)
        {
            var existing = Employees.FirstOrDefault(e =>
                e.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (existing == null)
                throw new InvalidInputException("Employee not found.");


            if (!existing.Username.Equals(updated.Username, StringComparison.OrdinalIgnoreCase))
            {
                if (Employees.Any(e =>
                    e.Username.Equals(updated.Username, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new Exception("Username already exists.");
                }
            }

            existing.Username = updated.Username;
            existing.Password = updated.Password;
            existing.PersonalInfo = updated.PersonalInfo;

            _jsonHelper.Save(Employees);
        }
    }
}
