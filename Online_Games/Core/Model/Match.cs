using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Match : BaseModel<int>
    {
        public int TournamentId { get; set; }
        public Tournament? Tournament { get; set; }

        public int GameId { get; set; }
        public Game? Game { get; set; }

        public int Player1Id { get; set; }
        public int Player2Id { get; set; }

        public int RoundId { get; set; }
        public Round? Round { get; set; }

        public string Status { get; set; } = "Scheduled";

        public int? WinnerUserId { get; set; }
        public int? LooserUserId { get; set; }
        
        public string? Score { get; set; }
        public DateTime MatchDate { get; set; }

        public Match(int id = 0) : base(id) { }

        public Match(int id, DateTime createdAt, DateTime? lastUpdatedAt, bool isActive, int tournamentId,
            int gameId, int player1Id, int player2Id, int roundId, string? status, int? winnerUserId,
            int? looserUserId) : base(id, createdAt, lastUpdatedAt, isActive)
        {
            TournamentId = tournamentId;
            GameId = gameId;
            Player1Id = player1Id;
            Player2Id = player2Id;
            RoundId = roundId;
            Status = status ?? "Scheduled";
            WinnerUserId = winnerUserId;
            LooserUserId = looserUserId;          
        }

        public string GetMatchSummary()
        {
            string winner = (WinnerUserId > 0) ? $"Vencedor ID: {WinnerUserId}" : "N/A";
            string looser = (LooserUserId > 0) ? $"Perdedor ID: {LooserUserId}" : "N/A";

            return $"Match ID: {this.Id} | Jogo: {this.Game?.TypeGame ?? "N/A"} | Data: {this.MatchDate.ToString("dd/MM/yyyy")} | Score: {this.Score ?? "N/A"}\n" +
                   $"- {winner}, {looser} | Torneio ID: {this.Tournament?.Id ?? 0}, Ronda ID: {this.Round?.Id ?? 0}";
        }
    }
}
