using AppService.Interfaces;
using Core.Model;
using Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppService.Services
{
    public class RoundService : IRoundService
    {
        private readonly IRoundRepository _roundRepository;
        private readonly IMatchService _matchService;

        public RoundService(IRoundRepository roundRepository, IMatchService matchService)
        {
            _roundRepository = roundRepository;
            _matchService = matchService;
        }

        public Round StartNewRound(int matchId)
        {
            if(matchId <= 0)
            {
                throw new ArgumentException("ID da partida inválido para iniciar a Ronda.", nameof(matchId));
            }

            Match? match = _matchService.GetMatchById(matchId);

            if(match == null)
            {
                throw new KeyNotFoundException($"Partida com ID {matchId} não encontrada. Não é possível iniciar a ronda.");
            }
            if(match.Status != "InProgress")
            {
                throw new InvalidOperationException("Não é possível iniciar uma ronda numa partida que não esteja em progresso.");
            }

            List<Round> existingRounds = _roundRepository.GetRoundsByMatch(matchId);
            int nextRoundNumber = existingRounds.Count + 1;

            var newRound = new Round(
                tournamentId: match.TournamentId,
                gameId: match.GameId,
                matchId: matchId,
                country: null,
                roundNumber: nextRoundNumber
            );

            _roundRepository.Create(newRound);
            return newRound;
        }

        public Round EndCurrentRound(int roundId, Dictionary<int, int> scores)
        {
            if(roundId <= 0)
            {
                throw new ArgumentException("ID da ronda inválido para finalizar.", nameof(roundId));
            }
            if(scores == null || !scores.Any())
            {
                throw new ArgumentException("As pontuações são obrigatórias para finalizar a ronda.", nameof(scores));
            }

            Round? round = _roundRepository.Retrieve(roundId);

            if(round == null)
            {
                throw new KeyNotFoundException($"Ronda com ID {roundId} não encontrada.");
            }

            int maxScore = scores.Values.Max();
            round.WinnerIds = scores.Where(kv => kv.Value == maxScore)
                                    .Select(kv => kv.Key)
                                    .ToList();
            
            round.LooserIds = scores.Where(kv => kv.Value < maxScore)
                                    .Select(kv => kv.Key)
                                    .ToList();

            round.Country = "Resultado Calculado";

            _roundRepository.Update(round);
            _matchService.ProcessRoundResult(round.MatchId, round.WinnerIds);
            return round;
        }

        public List<Round> GetRoundsByMatch(int matchId)
        {
            if (matchId <= 0)
            {
                throw new ArgumentException("ID da partida inválido.", nameof(matchId));
            }            
            return _roundRepository.GetRoundsByMatch(matchId);
        }
    }
}
