using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;

namespace Repo.Interfaces
{
    public interface ITournamentRepository : IBaseRepository<Tournament>
    {
        List<Tournament> GetUpcomingTournaments();
        List<Tournament> GetActiveTournaments();

        bool IsPlayerRegistered(int tournamentId, int userId);
        void RegisterPlayer(int tournamentId, int userId);
        List<int> GetRegisteredPlayerIds(int tournamentId);
        int? GetFinalWinner(int tournamentId);
    }
}