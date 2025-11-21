using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;
using Data.Interfaces;

namespace Online_Game.Data.Interfaces
{
    public interface IMatchRepository : IBaseRepository<Match>
    {
        List<Match> GetUpcomingMatches();
        List<Match> GetRecentMatches(int count);
        List<Match> GetPlayerHistory(int userId);
    }
}