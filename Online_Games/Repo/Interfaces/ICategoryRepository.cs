using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;

namespace Repo.Interfaces
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {    

        Category? GetByName(string categoryName);
        bool ExistsWithName(string categoryName, int? excludeId = null);
    }
}
