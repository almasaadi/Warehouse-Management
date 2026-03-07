using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Contracts
{
    public interface IUserValidationService
    {
        void ValidateLoginInput(string username, string password);
    }

}
