using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppService.Interfaces
{
    public interface IRoundService
    {
        Round StartNewRound(int matchId);
        Round EndCurrentRound(int roundId, Dictionary<int, int> scores);
        List<Round> GetRoundsByMatch(int matchId);        
    }
}
