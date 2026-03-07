using System;
using System.Linq;
using ManagmentSystem.Models;
using ManagmentSystem.Data;
using ManagmentSystem.Exceptions;
using ManagmentSystem.Contracts;

namespace ManagmentSystem.Services
{
    public class LoginService
    {
        private readonly JsonHelper<Employee> _storage;
        private readonly IUserValidationService _validationService;

        public LoginService()
        {
            // نفس ملف الموظفين لضمان توحيد مصدر البيانات
            _storage = new JsonHelper<Employee>("employees.json");

            _validationService = new UserValidationService();
        }

        public Employee Authenticate(string username, string password)
        {
            // 1) تحقق من مدخلات تسجيل الدخول فقط
            _validationService.ValidateLoginInput(username, password);

            // 2) تحميل المستخدمين
            var users = _storage.Load();

            // 3) البحث عن المستخدم
            var user = users.FirstOrDefault(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                u.Password == password);

            if (user == null)
                throw new AuthenticationException("Invalid username or password.");

            return user;
        }
    }
}
