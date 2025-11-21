using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppService.Interfaces
{
    public interface ITournamentService
    {
        List<Tournament> GetUpcomingTournaments();
        List<Tournament> GetActiveTournaments();
        void RegisterPlayer(int tournamentId, int userId);
        Tournament StartTournament(int tournamentId);
        Tournament EndTournament(int tournamentId);
    }
}
