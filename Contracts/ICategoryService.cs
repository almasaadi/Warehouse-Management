using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagmentSystem.Models;

namespace AD_project.Contracts
{
    public interface ICategoryService
    {
        List<Category> GetAllCategories();
        void AddCategory(string name);
        void UpdateCategory(int id, string newName);
        void DeleteCategory(int id);
    }
}
