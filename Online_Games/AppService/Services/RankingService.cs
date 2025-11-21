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
    public class RankingService : IRankingService
    {
        private readonly IRankingRepository _rankingRepository;
        private readonly IMatchService _matchService;

        public RankingService(IRankingRepository rankingRepository, IMatchService matchService)
        {
            _rankingRepository = rankingRepository;
            _matchService = matchService;
        }

        public void AddNewPlayerToGame(int userId, int gameId)
        {
            if(userId <= 0 || gameId <= 0)
            {
                throw new ArgumentException("ID's de utilizador e jogo deve ser positivos.");
            }

            Ranking? existingRanking = _rankingRepository.GetRankingByUserIdAndGameId(userId, gameId);

            if(existingRanking != null)
            {
                throw new InvalidOperationException($"O utilizador {userId} já possui um ranking no jogo {gameId}.");
            }

            // Lógica de Negócio: Definir os valores iniciais (1000 MMR, 0 Vitórias/Derrotas)
            const int INITIAL_MMR = 1000;

            Ranking newRanking = new Ranking(id: 0)
            {
                UserId = userId,
                GameId = gameId,
                MMRValue = INITIAL_MMR,
                Wins = 0,
                Losses = 0,
                LastUpdate = DateTime.Now
            };

            _rankingRepository.Create(newRanking);
        }

        public List<Ranking> GetGlobalLeaderboard(int limit)
        {
            if (limit <= 0)
            {
                limit = 100; // Limite padrão
            }

            // Consulta ao Repositório para o Leaderboard Global (SQL ORDER BY MMR)
            return _rankingRepository.GetGlobalLeaderboardData(limit);
        }

        public List<Ranking> GetGameLeaderboard(int gameId, int limit)
        {
            if (gameId <= 0)
            {
                throw new ArgumentException("ID do jogo inválido.", nameof(gameId));
            }
            if (limit <= 0)
            {
                limit = 50; // Limite padrão
            }

            // 2. Consulta ao Repositório para o Leaderboard por Jogo (SQL WHERE GameId = @id ORDER BY MMR)
            return _rankingRepository.GetGameLeaderboardData(gameId, limit);
        }

        public Ranking? GetUserRanking(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("ID do utilizador inválido.", nameof(userId));
            }            

            List<Ranking> userRankings = _rankingRepository.GetAllUserRankings(userId);

            if (!userRankings.Any())
            {
                return null;
            }
            
            int totalMMR = userRankings.Sum(r => r.MMRValue);
            int totalWins = userRankings.Sum(r => r.Wins);
            int totalLosses = userRankings.Sum(r => r.Losses);
                       
            return new Ranking(0)
            {
                UserId = userId,
                MMRValue = totalMMR,
                Wins = totalWins,
                Losses = totalLosses                
            };
        }

        public void RecalculateRankings()
        {     
            Console.WriteLine("Iniciado o processo de RecalculateRankings. Será executado em background.");        
        }

        public void RecalculateUserRanking(int matchId, int winnerUserId, int looserUserId)
        {
            if (matchId <= 0 || winnerUserId <= 0 || looserUserId <= 0)
            {
                throw new ArgumentException("IDs de partida, vencedor e perdedor devem ser positivos.");
            }

            // ORQUESTRAÇÃO: Obter detalhes da Match para determinar o GameId
            // Assumimos que o IMatchService foi atualizado para incluir Match? GetMatchById(int matchId);
            Match? match = _matchService.GetMatchById(matchId);

            if (match == null)
            {
                throw new KeyNotFoundException($"Partida com ID {matchId} não encontrada para recálculo de ranking.");
            }

            int gameId = match.GameId;

            Ranking? winnerRanking = _rankingRepository.GetRankingByUserIdAndGameId(winnerUserId, gameId);
            Ranking? looserRanking = _rankingRepository.GetRankingByUserIdAndGameId(looserUserId, gameId);

            winnerRanking ??= new Ranking { UserId = winnerUserId, GameId = gameId, MMRValue = 1000 };
            looserRanking ??= new Ranking { UserId = looserUserId, GameId = gameId, MMRValue = 1000 };

            // LÓGICA DE NEGÓCIO: Aplicar a fórmula (MMR/Elo) Entender melhor este campo
            int kFactor = 32; // Constante de ajuste (exemplo)

            int mmrChange = CalculateMMRChange(winnerRanking.MMRValue, looserRanking.MMRValue, kFactor);

            winnerRanking.MMRValue += mmrChange;
            looserRanking.MMRValue -= mmrChange; // O valor é subtraído ao perdedor

            winnerRanking.Wins += 1;
            looserRanking.Losses += 1;

            _rankingRepository.Update(winnerRanking);
            _rankingRepository.Update(looserRanking);

            // Exemplo Simplificado:
            Console.WriteLine($"Recalculando ranking para Vencedor ID: {winnerUserId} e Perdedor ID: {looserUserId}");
        }

        private int CalculateMMRChange(int winnerMMR, int looserMMR, int kFactor)
        {
            // Implementação simplificada da lógica MMR/Elo. 
            // Fórmula base: ExpectedScore = 1 / (1 + 10^((MMR_B - MMR_A) / 400))

            double ratingDifference = (double)looserMMR - winnerMMR;
            double expectedWin = 1.0 / (1.0 + Math.Pow(10.0, ratingDifference / 400.0));

            int change = (int)Math.Round(kFactor * (1 - expectedWin));

            return Math.Max(1, change);
        }
    }
}
