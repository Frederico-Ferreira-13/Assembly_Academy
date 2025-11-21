using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Game : BaseModel<int>
    {
        public string TypeGame { get; set; } = string.Empty;
        // Foreign Key para a tabela Category
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public Game() : base(0) { }

        public Game(int id, DateTime createdAt, DateTime? lastUpdatedAt, bool isActive, string typeGame, int categoryId) : base(id)
        {
            TypeGame = typeGame;
            CategoryId = categoryId;
        }
    }
}
