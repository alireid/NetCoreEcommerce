using System.Collections.Generic;
using NetCoreEcommerce.Data.Models;

namespace NetCoreEcommerce.Data
{
    public interface ICategory
    {
        IEnumerable<Category> GetAll();
        Category GetById(int id);
        void NewCategory(Category category);
        void EditCategory(Category category);
        void DeleteCategory(int id);
    }
}
