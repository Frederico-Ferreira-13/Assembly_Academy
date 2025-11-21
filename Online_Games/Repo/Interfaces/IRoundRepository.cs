using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;


namespace Repo.Interfaces
{
    public interface IRoundRepository : IBaseRepository<Round>
    {
        List<Round> GetRoundsByMatch(int matchId);
    }
}