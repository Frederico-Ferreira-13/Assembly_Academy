using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppService.Interfaces
{
    public interface IRankingService
    {
        void AddNewPlayerToGame(int userId, int gameId);
        List<Ranking> GetGlobalLeaderboard(int limit);
        List<Ranking> GetGameLeaderboard(int gameId, int limit);
        Ranking? GetUserRanking(int userId);
        void RecalculateRankings();
        void RecalculateUserRanking(int matchId, int winnerUserId, int looserUserId);       
    }
}
