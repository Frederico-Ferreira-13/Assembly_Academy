using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppService.Interfaces
{
    public interface IGameService
    {
        void CreateGame(Game model);
        Game? GetGameById(int id);
        List<Game> GetAllGames();
        void UpdateGame(Game model);
        bool RemoveGame(int id);

        List<Game> SearchGames(string searchTerm);
        List<Game> GetGamesByGenre(int genreId);
        bool IsTitleUnique(string title, int? excludeId = null);
    }
}
