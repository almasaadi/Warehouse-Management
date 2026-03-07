using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagmentSystem.Models;

namespace AD_project.Contracts
{
    public  interface IProductValidator
    {
        void ValidateForCreateOrUpdate(Product product);
    }
}
