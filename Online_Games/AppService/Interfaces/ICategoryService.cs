using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppService.Interfaces
{
    public interface ICategoryService
    {
        void RegisterNewCategory(Category category);
        void UpdateCategory(Category category);
        bool RemoveCategory(int id);

        Category GetCategoryDetails(int id);
        List<Category> GetCategoryList();

        Category? GetByName(string categoryName);
        bool IsCategoryNameUnique(string categoryName, int? excludeId = null);
        List<Category> GetActiveCategories();
    }
}
