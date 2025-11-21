using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Category : BaseModel<int>    
    {        
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public List<Game> Games { get; set; } = new List<Game>(); // Uma Categoria pode ter multiplos Games - Relação 1:N

        public Category() : base(0) { }

        public Category(string? categoryName, string? description) : base(0)
        {
            CategoryName = categoryName;
            Description = description;
        }

        public Category(int id, DateTime createdAt, DateTime? lastUpdatedAt, bool isActive,
            string? categoryName, string? description) : base(id, createdAt, lastUpdatedAt, isActive)
        {
            CategoryName = categoryName;
            Description = description;
        }
    }
}
