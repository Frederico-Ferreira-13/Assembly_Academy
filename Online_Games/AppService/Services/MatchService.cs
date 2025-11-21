using AppService.Interfaces;
using Core.Model;
using Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppService.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepository;
        private readonly IRankingService _rankingService;

        // Regra de Negócio: Quantas rondas são necessárias para vencer uma partida.
        private const int ROUNDS_TO_WIN = 3;

        public MatchService(IMatchRepository matchRepository, IRankingService rankingService)
        {
            _matchRepository = matchRepository;
            _rankingService = rankingService;
        }

        public void Create(Match match)
        {
            // Validações de criação da Match (ex: Players != null, TournamentId > 0)
            if (match == null)
            {
                throw new ArgumentException("A Match não pode ser nula.");
            }
            _matchRepository.Create(match);
        }

        public Match? GetMatchById(int matchId)
        {
            if (matchId <= 0) return null;
            return _matchRepository.Retrieve(matchId);
        }

        public bool RemoveMatch(int matchId)
        {
            if (matchId <= 0)
            {
                throw new ArgumentException("O ID da partida deve ser positivo.", nameof(matchId));
            }

            try
            {
                return _matchRepository.Delete(matchId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao remover a partida com ID {matchId}: {ex.Message}", ex);
            }
        }

        public List<Match> GetUpcomingMatches()
        {            
            return _matchRepository.GetUpcomingMatches();
        }

        public List<Match> GetRecentMatches(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentException("A contagem deve ser um número inteiro positivo.", nameof(count));
            }
            
            return _matchRepository.GetRecentMatches(count);
        }

        public List<Match> GetPlayerHistory(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("O ID do utilizador deve ser um número inteiro positivo.", nameof(userId));
            }
            
            return _matchRepository.GetPlayerHistory(userId);
        }

        public Match StartMatch(int matchId)
        {
            if (matchId <= 0)
            {
                throw new ArgumentException("ID da partida inválido.", nameof(matchId));
            }
            
            Match? match = _matchRepository.Retrieve(matchId);

            if (match == null)
            {
                throw new KeyNotFoundException($"Partida com ID {matchId} não encontrada.");
            }

            match.Status = "In Progress";

            _matchRepository.Update(match);

            return match;
        }

        public void ProcessRoundResult(int matchId, List<int> roundWinnerIds)
        {            
            Match? match = _matchRepository.Retrieve(matchId);

            if (match == null)
            {
                throw new KeyNotFoundException($"Partida com ID {matchId} não encontrada.");
            }
            if(roundWinnerIds.Count != 1)
            {
                return;
            }

            int roundWinnerId = roundWinnerIds.First();      

            _matchRepository.Update(match);          
        }

        public Match EndMatch(int matchId, int winnerId)
        {
            if (matchId <= 0 || winnerId <= 0)
            {
                throw new ArgumentException("IDs inválidos.", nameof(matchId));
            }

            Match? match = _matchRepository.Retrieve(matchId);

            if (match == null)
            {
                throw new KeyNotFoundException($"Partida com ID {matchId} não encontrada.");
            }

            // Lógica de Negócio: Atribuir Vencedor/Perdedor e Estado
            if (winnerId != match.Player1Id && winnerId != match.Player2Id)
            {
                throw new InvalidOperationException("O vencedor não é um participante desta partida.");
            }

            match.WinnerUserId = winnerId;
            match.LooserUserId = (winnerId == match.Player1Id) ? match.Player2Id : match.Player1Id;
            match.Status = "Completed";

            _matchRepository.Update(match);

            // usa-se .Value (ou !) sempre que o código se move de um contexto anulável (int?) para um
            // contexto não anulável (int) onde a sua lógica de negócio garante a presença de um valor.
            _rankingService.RecalculateUserRanking(matchId, match.WinnerUserId!.Value, match.LooserUserId!.Value);

            return match;
        }
    }
}
