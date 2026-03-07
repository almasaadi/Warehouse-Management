using ManagmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Contracts
{
    public interface IEmployeeManager
    {
        void AddEmployee(Employee employee);
        void RemoveEmployee(string username);
        void UpdateEmployee(string username,Employee updatedEmployee);
        List<Employee> GetAllEmployees();

    }
}
