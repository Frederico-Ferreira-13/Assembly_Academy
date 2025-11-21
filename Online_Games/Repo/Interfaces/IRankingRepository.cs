using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;

namespace Repo.Interfaces
{
    public interface IRankingRepository : IBaseRepository<Ranking>
    {
        List<Ranking> GetGlobalLeaderboardData(int limit);
        List<Ranking> GetGameLeaderboardData(int gameId, int limit);
        Ranking? GetRankingByUserIdAndGameId(int userId, int gameId);
        Ranking? GetUserRankingByGame(int userId, int gameId);
        List<Ranking> GetAllUserRankings(int userId);
        void RecalculateUserRanking(int winnerUserId, int looserUserId);
    }
}