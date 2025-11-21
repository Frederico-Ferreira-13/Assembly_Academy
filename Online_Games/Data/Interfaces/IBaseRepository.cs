using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IBaseRepository<TModel> where TModel : BaseModel<int>
    {
        void Create(TModel model);
        TModel? Retrieve(int id);
        List<TModel> RetrieveAll();
        void Update(TModel model);
        bool Delete(int id);
    
        Task<int> SaveChangesAsync();
    }
}
