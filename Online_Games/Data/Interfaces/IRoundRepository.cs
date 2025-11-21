using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;
using Data.Interfaces;

namespace Online_Game.Data.Interfaces
{
    public interface IRoundRepository : IBaseRepository<Round>
    {
        List<Round> GetRoundsByMatch(int matchId);
    }
}