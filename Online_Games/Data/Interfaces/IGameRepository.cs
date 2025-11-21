using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;
using Data.Interfaces;

namespace Online_Game.Data.Interfaces
{
    public interface IGameRepository : IBaseRepository<Game>
    {
        List<Game> Search(string searchTerm);
        List<Game> GetByGenre(int genreId);
        bool IsTitleUnique(string title, int? excludeId = null);
    }
}