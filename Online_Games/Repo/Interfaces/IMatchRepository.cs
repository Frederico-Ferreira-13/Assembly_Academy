using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;

namespace Repo.Interfaces
{
    public interface IMatchRepository : IBaseRepository<Match>
    {
        List<Match> GetUpcomingMatches();
        List<Match> GetRecentMatches(int count);
        List<Match> GetPlayerHistory(int userId);
    }
}