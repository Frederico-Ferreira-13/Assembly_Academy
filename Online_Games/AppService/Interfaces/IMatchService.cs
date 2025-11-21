using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppService.Interfaces
{
    public interface IMatchService
    {
        void Create(Match match);
        Match? GetMatchById(int matchId);
        bool RemoveMatch(int matchId);

        List<Match> GetUpcomingMatches();
        List<Match> GetRecentMatches(int count);
        Match StartMatch(int matchId);        
        List<Match> GetPlayerHistory(int userId);
        Match EndMatch(int matchId, int winnerId);
        void ProcessRoundResult(int matchId, List<int> roundWinnerIds);
    }
}
